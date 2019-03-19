/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            3/10/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Conversions;
using EasyLab.Server.Data.Models;
using EasyLab.Server.Repository.Interface;
using NLog;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Business.Validators;
using System.Net.Http;
using System.Net;
using System.Configuration;
using EasyLab.Server.Resources;

namespace EasyLab.Server.Business
{
    public abstract class MessageAppSvcBase : IMessageAppSvc
    {
        #region Static Members

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constants

        public const string EasyLab_Max_Retry_Count = "max_retry_count";
        public const int EasyLab_Default_Max_Retry_Count = 5;
        protected const string EasyLab_Message_Batch_Size = "easylab_message_batch_size";
        protected const int EasyLab_Default_Message_Batch_Size = 1000;
        protected const string EasyLab_Server_API_Address = "easylab_server_api_address";

        #endregion

        #region Fields

        protected IUnitOfWork unitOfWork;
        protected IRepository<Message> messageRepository;
        protected IRepository<GlobalSetting> globalRepository;
        protected IRepository<AuditLog> auditLogRepository;
        protected IRepository<User> userRepository;
        protected IRepository<LabInstrument> labInstrumentRepository;
        protected IRepository<ReserveQueue> reserveQueueRepository;

        #endregion

        public MessageAppSvcBase(IUnitOfWork unitOfWork
            , IRepository<Message> messageRepository
            , IRepository<GlobalSetting> globalRepository
            , IRepository<AuditLog> auditLogRepository
            , IRepository<User> userRepository
            , IRepository<LabInstrument> labIntrumentRepository
            , IRepository<ReserveQueue> reserveQueueRepository)
        {
            this.unitOfWork = unitOfWork;

            this.messageRepository = messageRepository;
            this.messageRepository.UnitOfWork = unitOfWork;

            this.globalRepository = globalRepository;
            this.globalRepository.UnitOfWork = unitOfWork;

            this.auditLogRepository = auditLogRepository;
            this.auditLogRepository.UnitOfWork = unitOfWork;

            this.userRepository = userRepository;
            this.userRepository.UnitOfWork = unitOfWork;

            this.labInstrumentRepository = labIntrumentRepository;
            this.labInstrumentRepository.UnitOfWork = unitOfWork;

            this.reserveQueueRepository = reserveQueueRepository;
            this.reserveQueueRepository.UnitOfWork = unitOfWork;
        }

        public DTOs.Message Get(Guid id)
        {
            var data = messageRepository.GetByKey(id);

            return data.ToDto();
        }

        public void Create(DTOs.Message dto)
        {
            ThrowHelper.ThrowArgumentNullExceptionIfNull(dto, "dto");

            var validator = new MessageValidator();
            validator.ValidateAndThrowEasyLabException(dto);

            var data = dto.ToData();
            data.MessageId = IdentityGenerator.NewSequentialGuid();
            data.EntryDate = DateTime.UtcNow;

            unitOfWork.ProcessWithTransaction(() => messageRepository.Add(data));

            dto.Id = data.MessageId;
            dto.EntryDate = data.EntryDate;
        }


        #region Handler Message Exception

        protected static void HandleAggregateException(ref bool networkAvalilabe, Message message, Exception exception)
        {
            if (exception is HttpRequestException && exception.InnerException != null && exception.InnerException is WebException)
            {
                var webException = exception.InnerException as WebException;

                if (webException.Status == WebExceptionStatus.NameResolutionFailure || webException.Status == WebExceptionStatus.ConnectFailure)
                {
                    logger.Trace<WebException>(webException);
                    networkAvalilabe = false;

                    return;
                }
            }
            HandleGenericException(message, exception);
        }

        protected static void HandleGenericException(Message message, Exception ex)
        {
            UpdateMessageStatus(message, MessageResult.Fail);

            logger.Error<Guid, Exception>(Rs.LOG_MESSAGE_FAILED_WITH_EXCEPTION, message.MessageId, ex);
        }

        protected static void UpdateMessageStatus(Message message, MessageResult result)
        {
            if (result == MessageResult.Cancel)
            {
                return;
            }
            message.Processed = true;
            message.ProcessDate = DateTime.UtcNow;

            if (result == MessageResult.Success)
            {
                message.Failed = false;
            }
            else if (result == MessageResult.Fail)
            {
                if (!message.Failed)
                {
                    message.Failed = true;
                }
                else
                {
                    message.RetryCount++;
                }
            }
            else if (result == MessageResult.FailNoRetry)
            {
                message.Failed = false;
                message.RetryCount = 5;
            }
        }

        protected int GetNumberFromAppConfig(string key, int defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];

            int intValue;

            if (int.TryParse(value, out intValue) && intValue > 0)
            {
                return intValue;
            }

            return defaultValue;
        }

        #endregion
    }
}
