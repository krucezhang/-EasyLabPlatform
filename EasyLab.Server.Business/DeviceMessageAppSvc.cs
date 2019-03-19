
/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            3/06/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Common.Extensions;
using EasyLab.Server.Conversions;
using EasyLab.Server.Data.Models;
using EasyLab.Server.Repository.Interface;
using EasyLab.Server.Resources;
using EasyLab.Server.Response.Result;
using EasyLab.Server.Business.Validators;

namespace EasyLab.Server.Business
{
    public class DeviceMessageAppSvc : MessageAppSvcBase, IDeviceMessageAppSvc
    {
        IRepository<DeviceSetting> deviceSettingRepository;

        public DeviceMessageAppSvc(IUnitOfWork unitOfWork,
            IRepository<Message> messageRepository,
            IRepository<GlobalSetting> globalRepository,
            IRepository<DeviceSetting> deviceSettingRepository,
            IRepository<AuditLog> auditLogRepository,
            IRepository<User> userRepository,
            IRepository<LabInstrument> labIntrumentRepositroy,
            IRepository<ReserveQueue> reserveQueueRepository)
            : base(unitOfWork, messageRepository, globalRepository, auditLogRepository, userRepository, labIntrumentRepositroy, reserveQueueRepository) 
        {
            this.deviceSettingRepository = deviceSettingRepository;
            this.deviceSettingRepository.UnitOfWork = unitOfWork;
        }

        public void Process()
        {
            ProcessDevice((batchSize, maxRetryCount) =>
            {
                return (from message in this.messageRepository.GetQuery()
                        where !message.Processed
                        orderby message.EntryDate
                        select message).Take(batchSize).ToList();
            });
        }

        public void Retry()
        {
            ProcessDevice((batchSize, maxRetryCount) =>
            {
                return (from message in this.messageRepository.GetQuery()
                        where message.Processed && message.Failed && message.RetryCount < maxRetryCount
                        orderby message.EntryDate
                        select message).Take(batchSize).ToList();
            });
        }

        private void ProcessDevice(Func<int, int, IEnumerable<Message>> queryData)
        {
            logger.Info(Rs.LOG_MESSAGE_START_PROCESS);

            var maxRetryCount = this.GetNumberFromAppConfig(EasyLab_Max_Retry_Count, EasyLab_Default_Max_Retry_Count);
            var batchSize = this.GetNumberFromAppConfig(EasyLab_Message_Batch_Size, EasyLab_Default_Message_Batch_Size);
            //var restServerIpV4 = this.globalRepository.GetByKey("rest", "serverIpV4");
            //var restPort = this.globalRepository.GetByKey("rest", "serverPort");
            var restServerAddress = this.globalRepository.GetByKey("rest", "serverRestAddress");

            if (restServerAddress == null || string.IsNullOrWhiteSpace(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_GLOBAL_SETTING_NOT_READY);

                return;
            }

            if (!Helper.CanConnectServer(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_NETWORK_NOT_AVAILABLE);

                return;
            }

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {
                var networkAvailable = true;
                var messages = queryData(batchSize, maxRetryCount);

                logger.Info(Rs.LOG_MESSAGE_COUNT, messages.Count());

                foreach (var message in messages)
                {
                    if (!networkAvailable)
                    {
                        break;
                    }
                    try
                    {
                        ProcessMessages(client, message);
                    }
                    catch (AggregateException ae)
                    {
                        ae.Handle(x =>
                            {
                                HandleAggregateException(ref networkAvailable, message, x);

                                return true;
                            });
                    }
                    catch (Exception ex)
                    {
                        HandleGenericException(message, ex);
                    }
                    finally
                    {
                        this.unitOfWork.ProcessWithTransaction(() =>
                            {
                                messageRepository.Update(message);
                            });
                    }
                }
            }
        }

        private void ProcessMessages(ServiceClient client, Message message)
        {
            MessageResult result;

            switch ((MessageTypes)message.MessageType)
            {
                case MessageTypes.PostbackAuditLogs:
                    result = PostbackAuditLogs(client, message);
                    break;
                case MessageTypes.InitUsersDataFromServer:
                    result = GetUserInfoFromServer(client, message);
                    break;
                case MessageTypes.AddAndUpdateReserverQueue:
                    result = GetInstrumentReserveQueue(client, message);
                    break;
                case MessageTypes.SendReserveQueueInfo:
                    result = SendReserveQueueInfoToServer(client, message);
                    break;
                case MessageTypes.SendClientCancelReserve:
                    result = SendClientCancelReserveToServer(client, message);
                    break;
                default:
                    result = MessageResult.Cancel;
                    logger.Warn(Rs.LOG_MESSAGE_INVALID_TYPE, message.MessageType, message.MessageId);
                    break;
            }

            UpdateMessageStatus(message, result);
        }

        private MessageResult SendClientCancelReserveToServer(ServiceClient client, Message message)
        {
            var InstrumentId = GetStationIdFromDeviceSettings();
            if (InstrumentId == null)
            {
                return MessageResult.Cancel;
            }

            var reserveQueue = this.reserveQueueRepository.GetQuery().Where(s => s.queueId == new Guid(message.RecordId)).FirstOrDefault();
            if (reserveQueue == null)
            {
                return MessageResult.Fail;
            }

            var labInstrument = this.labInstrumentRepository.GetQuery().Where(s => s.RecordId == InstrumentId).FirstOrDefault();
            if (labInstrument == null)
            {
                return MessageResult.Fail;
            }

            var user = this.userRepository.GetByKey(reserveQueue.userId);
            if (user == null)
            {
                return MessageResult.Fail;
            }

            string[] userTag = message.Tag.Split(',');

            client.RequestUri = "ajaxReserveCancel.action?";

            CookieCollection cookies = new CookieCollection();

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("labId", labInstrument.ParentId);
            parameters.Add("loginName", userTag[0].Trim());
            parameters.Add("password", userTag[1].Trim());
            parameters.Add("instrumentId", InstrumentId);
            parameters.Add("reserveRecordId", reserveQueue.reserveId);

            HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

            string rs = Helper.ReadResponseStream(response);

            ReserveRecordResult responseJson = Helper.ResponseJsonSerializer<ReserveRecordResult>(rs);

            if (responseJson.errorType.IsSameAs("success"))
            {
                return MessageResult.Success;
            }

            return MessageResult.Fail;
        }

        private MessageResult SendReserveQueueInfoToServer(ServiceClient client, Message message)
        {
            var InstrumentId = GetStationIdFromDeviceSettings();
            if (InstrumentId == null)
            {
                return MessageResult.Cancel;
            }

            var reserveQueue = this.reserveQueueRepository.GetQuery().Where(s => s.queueId == new Guid(message.RecordId)).FirstOrDefault();
            if (reserveQueue == null)
            {
                return MessageResult.Fail;
            }

            client.RequestUri = "checkReserveInfo.action?";

            CookieCollection cookies = new CookieCollection();

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("reserveRecord.id", reserveQueue.reserveId);
            parameters.Add("reserveRecord.loginTime", reserveQueue.loginDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("reserveRecord.logoutTime", reserveQueue.logoutDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("comment", reserveQueue.comment);

            if (reserveQueue.Flag == (int)ReserveQueueFlag.ClientAutoCancel)
            {
                parameters.Add("reserveState", "3");
            }
            else
            {
                parameters.Add("reserveState", "0");
            }

            if (reserveQueue.IsTemporary)
            {
                var user = this.userRepository.GetByKey(reserveQueue.userId);
                if (user == null)
                {
                    return MessageResult.Fail;
                }
                parameters.Add("userInfoId", user.UserInfoId);
                parameters.Add("instrumentId", InstrumentId.ToString());
            }

            HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

            string rs = Helper.ReadResponseStream(response);

            ReserveRecordResult responseJson = Helper.ResponseJsonSerializer<ReserveRecordResult>(rs);

            if (responseJson.errorCode.IsSameAs("success"))
            {
                return MessageResult.Success;
            }

            return MessageResult.Fail;
        }

        private MessageResult GetInstrumentReserveQueue(ServiceClient client, Message message)
        {
            var InstrumentId = GetStationIdFromDeviceSettings();
            if (InstrumentId == null)
            {
                return MessageResult.Cancel;
            }

            client.RequestUri = "checkReserveList.action?";

            CookieCollection cookies = new CookieCollection();

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("instrumentId", InstrumentId);
            parameters.Add("stDate", message.Tag);

            HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

            string rs = Helper.ReadResponseStream(response);

            ReserveQueueResult responseJson = Helper.ResponseJsonSerializer<ReserveQueueResult>(rs);

            if (responseJson.errorCode.IsSameAs("success"))
            {
                ReserveQueueResult reserveQueueResult = new ReserveQueueResult();

                int maxSort = GetMaxQueue();

                List<DTOs.ReserveQueue> reserveQueues = reserveQueueResult.ToReserverQueue(responseJson, maxSort);

                unitOfWork.ProcessWithTransaction(() =>
                    {
                        foreach (var dto in reserveQueues)
                        {
                            var reserveQueue = dto.ToData();

                            var user = this.userRepository.GetQuery().Where(s => s.UserInfoId == dto.reserveUserId).FirstOrDefault();
                            if (user != null)
                            {
                                reserveQueue.userId = user.UserId;
                            }

                            reserveQueue.queueId = IdentityGenerator.NewSequentialGuid();
                            reserveQueue.loginDate = null;
                            reserveQueue.logoutDate = null;

                            if (this.reserveQueueRepository.GetQuery().Where(s => s.reserveId.Equals(dto.reserveId)).FirstOrDefault() == null)
                            {
                                this.reserveQueueRepository.Add(reserveQueue);
                            }
                        }
                    });
                return MessageResult.Success;
            }
            else
            {
                return MessageResult.Fail;
            }
        }

        private int GetMaxQueue()
        {
            int? sort = this.reserveQueueRepository.GetQuery().Max(s => s.Sequence);

            return sort.HasValue ? sort.Value : 1;
        }

        private MessageResult GetUserInfoFromServer(ServiceClient client, Message message)
        {
            var InstrumentId = GetStationIdFromDeviceSettings();
            if (InstrumentId == null)
            {
                return MessageResult.Cancel;
            }

            client.RequestUri = "allUserInfo.action?";

            CookieCollection cookies = new CookieCollection();

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("instrumentId", InstrumentId);

            HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

            string rs = Helper.ReadResponseStream(response);

            UsersResult responseJson = Helper.ResponseJsonSerializer<UsersResult>(rs);

            if (responseJson.errorType.IsSameAs("success") || responseJson.errorCode.IsSameAs("success"))
            {
                UsersResult userResult = new UsersResult();

                List<DTOs.User> users = userResult.ToUser(responseJson);

                unitOfWork.ProcessWithTransaction(() =>
                    {
                        foreach (var dto in users)
                        {
                            var user = dto.ToData();
                            user.UserId = IdentityGenerator.NewSequentialGuid();
                            if (this.userRepository.GetQuery().Where(s => s.UserInfoId.Equals(user.UserInfoId)).FirstOrDefault() == null)
                            {
                                userRepository.Add(user);
                            }

                        }
                    });

                return MessageResult.Success;
            }
            else
            {
                return MessageResult.Fail;
            }
        }

        private MessageResult PostbackAuditLogs(ServiceClient client, Message message)
        {
            var InstrumentId = GetStationIdFromDeviceSettings();
            if (InstrumentId == null)
            {
                return MessageResult.Cancel;
            }

            var auditlog = this.auditLogRepository.GetQuery()
                .Where(o => o.AuditLogId == new Guid(message.RecordId)).FirstOrDefault();

            if (auditlog == null)
            {
                throw new EasyLabException(string.Format(Rs.MESSAGE_INVALID_AUDITLOG_ID, message.RecordId, message.MessageId, message.MessageType));
            }

            if (auditlog.InstrumentId == null || auditlog.InstrumentId == string.Empty || auditlog.InstrumentId != InstrumentId)
            {
                unitOfWork.ProcessWithTransaction(() =>
                    {
                        auditlog.InstrumentId = InstrumentId;
                        auditLogRepository.Update(auditlog);
                    });
            }

            client.RequestUri = "addAuditLog.action?";

            var param = auditlog.ToDto();
            param.User = new DTOs.User { UserInfoId = auditlog.UserId }; 
            param.Log.CreateDate = DateTime.SpecifyKind(param.Log.CreateDate, DateTimeKind.Utc);

            
            //var response = client.Post<DTOs.AuditLog>(param);
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("auditLog.instrumentId", param.Log.InstrumentId);
            parameters.Add("auditLog.auditLogId", param.Log.Id.ToString());
            parameters.Add("auditLog.userId", param.User.UserInfoId.ToString());
            parameters.Add("auditLog.ApplicationId", param.Application.Id.ToString());
            parameters.Add("auditLog.ResourceType", param.Log.ResourceType);
            parameters.Add("auditLog.ResourceValue", param.Log.ResourceValue);
            parameters.Add("auditLog.ResourceAction", param.Log.ResourceAction);
            parameters.Add("auditLog.CreateDate", param.Log.CreateDate.ToString());
            parameters.Add("auditLog.ResourceType2", param.Log.ResourceType2);
            parameters.Add("auditLog.ResourceValue2", param.Log.ResourceValue2);

            logger.Debug(Rs.LOG_MESSAGE_AUDIT_LOG_CONTENT, auditlog.AuditLogId, auditlog.ResourceType, auditlog.ResourceAction, auditlog.ResourceValue);
        
            HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

            string rs = Helper.ReadResponseStream(response);

            AuditLogsResult responseJson = Helper.ResponseJsonSerializer<AuditLogsResult>(rs);

            if (responseJson.errorCode.IsSameAs("success"))
            {
                return MessageResult.Success;
            }
            else
            {
                return MessageResult.Fail;
            }
        }


        private string GetStationIdFromDeviceSettings()
        {
            var stationIdSetting = this.deviceSettingRepository.GetByKey("common", "InstrumentId");

            if (stationIdSetting == null || string.IsNullOrWhiteSpace(stationIdSetting.OptionValue))
            {
                return null;
            }

            return stationIdSetting.OptionValue;
        }
    }
}
