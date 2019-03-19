using EasyLab.Server.Business.Interface;
using EasyLab.Server.Business.Validators;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Common.Extensions;
using EasyLab.Server.Conversions;
using EasyLab.Server.DTOs;
using EasyLab.Server.Repository.Interface;
using EasyLab.Server.Resources;
using EasyLab.Server.Response.Result;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;

namespace EasyLab.Server.Business
{
    public class ReserveQueueSvc : IReserveQueueSvc
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Data.Models.GlobalSetting> globalRepository;
        private IRepository<Data.Models.ReserveQueue> reserveQueueRepository;
        private IRepository<Data.Models.User> userRepository;
        private IRepository<Data.Models.DeviceSetting> deviceSettingRepository;
        private IRepository<Data.Models.Message> messageRepository;
        private IRepository<Data.Models.LabInstrument> labInstrumentRepository;

        protected const string EasyLab_Server_API_Address = "easylab_server_api_address";
        #region Static Members

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        public ReserveQueueSvc(IUnitOfWork unitOfWork
            , IRepository<Data.Models.User> userRepository
            , IRepository<Data.Models.Message> messageRepository
            , IRepository<Data.Models.GlobalSetting> globalRepository
            , IRepository<Data.Models.DeviceSetting> deviceSettingRepository
            , IRepository<Data.Models.LabInstrument> labInstrumentRepository
            , IRepository<Data.Models.ReserveQueue> reserveQueueRepository)
        {
            this.unitOfWork = unitOfWork;

            this.reserveQueueRepository = reserveQueueRepository;
            this.reserveQueueRepository.UnitOfWork = unitOfWork;

            this.globalRepository = globalRepository;
            this.globalRepository.UnitOfWork = unitOfWork;

            this.userRepository = userRepository;
            this.userRepository.UnitOfWork = unitOfWork;

            this.messageRepository = messageRepository;
            this.messageRepository.UnitOfWork = unitOfWork;

            this.labInstrumentRepository = labInstrumentRepository;
            this.labInstrumentRepository.UnitOfWork = unitOfWork;

            this.deviceSettingRepository = deviceSettingRepository;
            this.deviceSettingRepository.UnitOfWork = unitOfWork;
        }

        public ReserveQueue Get(string id)
        {
            ReserveQueue reserve = null;
            try
            {
                var Gid = Guid.Parse(id);

                reserve = this.reserveQueueRepository.GetQuery().Where(s => s.queueId == Gid).FirstOrDefault().ToDto();
            }
            catch (Exception ex)
            {
                logger.Error("ReserveQueue Get Function Error {0}", ex.Message);
            }

            return reserve;
        }

        public IEnumerable<Queue> ReserveQueueList()
        {
            List<Queue> queueList = new List<Queue>();

            var deviceInstance = this.deviceSettingRepository.GetByKey("common", "InstrumentId");

            if (deviceInstance == null)
            {
                return queueList;
            }

            //Get Instrument Config
            var labInstrument = this.labInstrumentRepository.GetQuery(s => s.RecordId == deviceInstance.OptionValue).FirstOrDefault();
            if (labInstrument == null)
            {
                return queueList;
            }

            var lastReserver = GetPreviousReserveUser();

            var reserveQueueList = (from rQueue in this.reserveQueueRepository.GetQuery()
                                    where rQueue.Flag == (int)ReserveQueueFlag.Normal && rQueue.IsTemporary == false
                                    select rQueue).OrderBy(s => s.Sequence).ToList();

            foreach (var reserveItem in reserveQueueList)
            {
                bool isAddQueue = false;

                if (lastReserver != null)
                {
                    if (lastReserver.logoutDate < reserveItem.startDate)
                    {
                        if (DateTime.Compare(DateTime.Now, reserveItem.startDate.AddMinutes(labInstrument.cancelAfterTime)) <= 0)
                        {
                            isAddQueue = true;
                        }
                    }
                    else
                    {
                        var leftTime = (DateTime.Now - lastReserver.logoutDate.Value).Minutes;
                        if (leftTime <= labInstrument.cancelAfterTime)
                        {
                            isAddQueue = true;
                        }
                    }
                }
                else
                {
                    isAddQueue = true;
                }
                
                if (isAddQueue)
                {
                    var userQueueInfo = (from user in this.userRepository.GetQuery()
                                         where user.UserId == reserveItem.userId
                                         select user).FirstOrDefault();
                    if (userQueueInfo != null && reserveItem.loginDate == null)
                    {
                        var queue = new Queue { User = new User(), ReserveQueue = new ReserveQueue() };

                        queue.ReserveQueue = reserveItem.ToDto();
                        queue.User = userQueueInfo.ToDto();

                        queueList.Add(queue);
                    }
                }
            }

            return queueList;
        }

        public ReserveQueue GetPreviousReserveUser()
        {
            //the latest logout time
            var logoutTime = this.reserveQueueRepository.GetQuery().Max(s => s.logoutDate);

            return this.reserveQueueRepository.GetQuery().Where(s => s.logoutDate == logoutTime).FirstOrDefault().ToDto();

        }

        public ReserveQueue AdminCancelReserveRecord(ReserveQueue reserve, string pwd, string username)
        {
            ThrowHelper.ThrowArgumentNullExceptionIfNull(reserve, "dto");

            //Get current instrument Id
            var deviceValue = this.deviceSettingRepository.GetQuery().Where(s => s.Category == "common" && s.OptionKey == "InstrumentId").FirstOrDefault();

            if (deviceValue == null)
            {
                return null;
            }

            var reserveData = reserve.ToData();
            
            var reserveTmp = this.reserveQueueRepository.GetByKey(reserveData.queueId);
            if ( reserveTmp != null)
            {
                unitOfWork.ProcessWithTransaction(() =>
                    {
                        if (reserveData.Flag == (int)ReserveQueueFlag.ClientCancel)
                        {
                            reserveTmp.Flag = reserveData.Flag;
                        }

                        this.reserveQueueRepository.Update(reserveTmp);
                        if (!reserveTmp.IsTemporary)
                        {
                            string loginInfo = username + ',' + pwd;
                            //General message to send to server
                            AddOrUpdateReserveQueueByMessage(this.messageRepository, reserveTmp.queueId.ToString(), deviceValue.OptionValue, loginInfo, MessageTypes.SendClientCancelReserve);
                        }
                    });
            }
            return reserveTmp.ToDto();
        }

        public bool ProcessUpdateQueue()
        {
            var reserveQueueList = (from rQueue in this.reserveQueueRepository.GetQuery()
                                    where rQueue.Flag == (int)ReserveQueueFlag.Normal && rQueue.IsTemporary == false
                                    select rQueue).ToList();
            if (reserveQueueList.Count == 0)
            {
                return true;
            }
            //Get current instrument Id
            var deviceValue = this.deviceSettingRepository.GetQuery().Where(s => s.Category == "common" && s.OptionKey == "InstrumentId").FirstOrDefault();

            if (deviceValue == null)
            {
                return false;
            }
            try
            {
                unitOfWork.ProcessWithTransaction(() =>
                {
                    foreach (var item in reserveQueueList)
                    {
                        if (item.loginDate == null && item.endDate < DateTime.Now)
                        {
                            item.Flag = (int)ReserveQueueFlag.ClientAutoCancel;
                            this.reserveQueueRepository.Update(item);

                            //General message to send to server
                            AddOrUpdateReserveQueueByMessage(this.messageRepository, item.queueId.ToString(), deviceValue.OptionValue, MessageTypes.SendReserveQueueInfo);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Error<Exception>(Rs.VERITY_RESERVE_QUEUE_ERROR, ex);

                return false;
            }
            return true;
        }

        public ReserveQueue CreateOrUpdate(ReserveQueue dto)
        {
            ThrowHelper.ThrowArgumentNullExceptionIfNull(dto, "dto");

            var data = dto.ToData();

            //Get current instrument Id
            var deviceValue = this.deviceSettingRepository.GetQuery().Where(s => s.Category == "common" && s.OptionKey == "InstrumentId").FirstOrDefault();

            if (deviceValue == null)
            {
                return null;
            }

            //Update
            if (this.reserveQueueRepository.GetQuery().Where(s => s.queueId.Equals(data.queueId)).FirstOrDefault() != null)
            {
                var temp = this.reserveQueueRepository.GetByKey(data.queueId);

                unitOfWork.ProcessWithTransaction(() =>
                    {
                        if (temp != null && data.Flag == (int)ReserveQueueFlag.Normal)
                        {
                            temp.loginDate = data.loginDate;
                            temp.Flag = data.Flag;
                            temp.comment = data.comment;
                        }

                        if (temp != null && data.Flag == (int)ReserveQueueFlag.Success)
                        {
                            temp.logoutDate = data.logoutDate;
                            temp.Flag = data.Flag;
                            temp.comment = data.comment;
                        }

                        this.reserveQueueRepository.Update(temp);

                        //Success complete lab
                        if (temp.Flag == (int)ReserveQueueFlag.Success)
                        {
                            //General message to send to server
                            AddOrUpdateReserveQueueByMessage(this.messageRepository, temp.queueId.ToString(), deviceValue.OptionValue, MessageTypes.SendReserveQueueInfo);
                        }
                    });

                return temp.ToDto();
            }
            else
            {

                //Temporary user time do not in the reserve time.
                //Add reserve that usually it is temporary user
                data.reserveId = "0";
                data.startDate = DateTime.Now;
                data.endDate = DateTime.Now;
                data.cancelReserve = 0;
                data.autoCancelReserve = 0;
                data.Sequence = GetMaxQueue() + 1;
                data.comment = string.Empty;

                unitOfWork.ProcessWithTransaction(() =>
                {
                    this.reserveQueueRepository.Add(data);
                });

                return data.ToDto();

            }
        }

        public void AutoUpdateReserveQueueState(string instrumentId)
        {
            this.unitOfWork.ProcessWithTransaction(() =>
                {
                    var queues = this.reserveQueueRepository.GetQuery().Where(s => s.Flag == (int)ReserveQueueFlag.Normal);

                    foreach (var queue in queues)
                    {
                        if (((DateTime.Now - queue.startDate).Hours > 0 && (DateTime.Now - queue.startDate).Minutes > queue.cancelReserve) || DateTime.Now > queue.endDate)
                        {
                            queue.Flag = (int)ReserveQueueFlag.ClientAutoCancel;

                            this.reserveQueueRepository.Update(queue);

                            //General message to send to server
                            AddOrUpdateReserveQueueByMessage(this.messageRepository, queue.queueId.ToString(), instrumentId, MessageTypes.SendReserveQueueInfo); 
                        }
                    } 
                });
        }

        public string AddOrUpdateReserveQueue(string instrumentId)
        {
            //var restServerIpV4 = this.globalRepository.GetByKey("rest", "serverIpV4");
            //var restPort = this.globalRepository.GetByKey("rest", "serverPort");
            var restServerAddress = this.globalRepository.GetByKey("rest", "serverRestAddress");

            if (restServerAddress == null || string.IsNullOrWhiteSpace(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_GLOBAL_SETTING_NOT_READY);

                return string.Empty;
            }

            if (!Helper.CanConnectServer(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_NETWORK_NOT_AVAILABLE);

                return string.Empty;
            }

            //Get current instrument Id
            var deviceValue = this.deviceSettingRepository.GetQuery().Where(s => s.Category == "common" && s.OptionKey == "InstrumentId").FirstOrDefault();

            if (deviceValue == null)
            {
                return null;
            }

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {

                try
                {
                    client.RequestUri = "checkReserveList.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("instrumentId", instrumentId);
                    parameters.Add("stDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    ReserveQueueResult responseJson = Helper.ResponseJsonSerializer<ReserveQueueResult>(rs);

                    if (responseJson.errorCode.IsSameAs("success") || responseJson.errorType.IsSameAs("success"))
                    {
                        ReserveQueueResult reserveQueueResult = new ReserveQueueResult();
                        //Get Max Id for queue
                        int maxSort = GetMaxQueue();

                        List<DTOs.ReserveQueue> reserveQueues = reserveQueueResult.ToReserverQueue(responseJson, maxSort);

                        var instrumentOptions = this.labInstrumentRepository.GetQuery().Where(s => s.RecordId == deviceValue.OptionValue).FirstOrDefault();

                        if (instrumentOptions == null)
                        {
                            return null;
                        }

                        var reserveQueuesDb = this.reserveQueueRepository.GetQuery();

                        var deleteReserveQueue = new List<Data.Models.ReserveQueue>();

                        foreach (var reserveItem in reserveQueuesDb)
                        {
                            var isDelete = false;
                            foreach (var item in reserveQueues)
                            {
                                if (reserveItem.reserveId == item.reserveId)
                                {
                                    isDelete = true;
                                    break;
                                }
                            }
                            if (!isDelete && reserveItem.loginDate == null)
                            {
                                if (!reserveItem.IsTemporary && DateTime.Now > reserveItem.startDate)
                                {
                                    var leftTime = (DateTime.Now - reserveItem.startDate).Minutes;
                                    if (leftTime >= instrumentOptions.cancelAfterTime)
                                    {
                                        deleteReserveQueue.Add(reserveItem);
                                    }
                                }
                            }
                        }

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
                                reserveQueue.IsTemporary = false;

                                if (this.reserveQueueRepository.GetQuery().Where(s => s.reserveId.Equals(dto.reserveId)).FirstOrDefault() == null)
                                {
                                    this.reserveQueueRepository.Add(reserveQueue);
                                }
                                else
                                {
                                    //if user has been login client, so it does not update reserve info
                                    if (this.reserveQueueRepository.GetQuery().Where(s => s.reserveId.Equals(dto.reserveId) && s.loginDate == null) != null)
                                    {
                                        this.reserveQueueRepository.Update(reserveQueue);
                                    }
                                }
                            }
                            foreach (var item in deleteReserveQueue)
                            {
                                this.reserveQueueRepository.Delete(item);
                            }
                        });
                        return Rs.GETLABINSTRUMENTLISTINITSUCCESS;
                    }
                    else
                    {
                        return Rs.GETLABINSTRUMENTLISTINITERROR;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<Exception>(Rs.VERITY_RESERVE_QUEUE_ERROR, ex);

                    return string.Empty;
                }
            }
        }

        private int GetMaxQueue()
        {
            int? sort = this.reserveQueueRepository.GetQuery().Max(s => s.Sequence);

            return sort.HasValue ? sort.Value : 1;
        }

        internal void AddOrUpdateReserveQueueByMessage(IRepository<Data.Models.Message> messageRepository, string recordId, string instrumentId, params MessageTypes[] types)
        {
            messageRepository.NewMessage(recordId, instrumentId, Constants.SendToServerMachineId, types);
        }

        internal void AddOrUpdateReserveQueueByMessage(IRepository<Data.Models.Message> messageRepository, string recordId, string instrumentId, string loginInfo, params MessageTypes[] types)
        {
            messageRepository.NewMessage(recordId, instrumentId, Constants.SendToServerMachineId, loginInfo, types);
        }
    }
}
