using EasyLab.Server.Business.Interface;
using EasyLab.Server.Conversions;
using EasyLab.Server.Repository.Interface;
using EasyLab.Server.Resources;
using NLog;
using System.Net;
using System.Net.Sockets;

namespace EasyLab.Server.Business
{
    public class DeviceSettingAppSvc : IDeviceSettingAppSvc
    {
        #region Static Members

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion
        private IUnitOfWork unitOfWork;
        private IRepository<Data.Models.DeviceSetting> deviceSettingRepository;
        private IRepository<Data.Models.GlobalSetting> globalSettingRepository;
        private IRepository<Data.Models.AuditLog> auditLogRepository;

        public DeviceSettingAppSvc(IUnitOfWork unitOfWork
            , IRepository<Data.Models.AuditLog> auditLogRepository
            , IRepository<Data.Models.DeviceSetting> deviceSettingRepository
            , IRepository<Data.Models.GlobalSetting> globalSettingRepository)
        {
            this.unitOfWork = unitOfWork;

            this.auditLogRepository = auditLogRepository;
            this.auditLogRepository.UnitOfWork = unitOfWork;

            this.deviceSettingRepository = deviceSettingRepository;
            this.deviceSettingRepository.UnitOfWork = unitOfWork;

            this.globalSettingRepository = globalSettingRepository;
            this.globalSettingRepository.UnitOfWork = unitOfWork;
        }

        public bool CanCanConnectServer()
        {
            var restServerAddress = this.globalSettingRepository.GetByKey("rest", "serverRestAddress");

            if (restServerAddress == null || string.IsNullOrWhiteSpace(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_GLOBAL_SETTING_NOT_READY);

                return false;
            }

            if (!IsExistNet())
            {
                logger.Warn(Rs.LOG_MESSAGE_NETWORK_NOT_AVAILABLE);

                return false;
            }
            return true;
        }

        private bool IsExistNet()
        {
            try
            {
                IPHostEntry dummy = Dns.GetHostEntry("www.baidu.com"); //using System.Net;
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public void AddOrUpdate(DTOs.DeviceSetting dto)
        {
            this.unitOfWork.ProcessWithTransaction(() =>
                {
                    var data = dto.ToData();

                    var orgData = this.deviceSettingRepository.GetByKey(data.Category, data.OptionKey);
                    if (orgData == null)
                    {
                        this.deviceSettingRepository.Add(data);
                    }
                    else
                    {
                        orgData.OptionValue = data.OptionValue;
                        this.deviceSettingRepository.Update(orgData);
                    }
                });
        }

        public void DeleteDeviceConfig(DTOs.DeviceSetting dto)
        {
            this.unitOfWork.ProcessWithTransaction(() =>
                {
                    var data = dto.ToData();

                    var entity = this.deviceSettingRepository.GetByKey(data.Category, data.OptionKey);

                    if (entity != null)
                    {
                        this.deviceSettingRepository.Delete(entity);
                    }
                });
        }

        public DTOs.DeviceSetting Get(string category, string optionKey)
        {
            return this.deviceSettingRepository.GetByKey(category, optionKey).ToDto();
        }
    }
}
