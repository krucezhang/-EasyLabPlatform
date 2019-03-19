using EasyLab.Server.Business.Interface;
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
    public class LabInstrumentAppSvc : ILabInstrumentAppSvc
    {
        protected const string EasyLab_Server_API_Address = "easylab_server_api_address";
        #region Static Members

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        private IUnitOfWork unitOfWork;
        private IRepository<Data.Models.GlobalSetting> globalRepository;
        private IRepository<Data.Models.LabInstrument> labInstrumentRepository;
        private IRepository<Data.Models.DeviceSetting> deviceSettingRepository;

        public LabInstrumentAppSvc(IUnitOfWork unitOfWork
            , IRepository<Data.Models.LabInstrument> labInstrument
            , IRepository<Data.Models.GlobalSetting> globalRepository
            , IRepository<Data.Models.DeviceSetting> deviceSettingRepository)
        {
            this.unitOfWork = unitOfWork;

            this.labInstrumentRepository = labInstrument;
            this.labInstrumentRepository.UnitOfWork = unitOfWork;

            this.globalRepository = globalRepository;
            this.globalRepository.UnitOfWork = unitOfWork;

            this.deviceSettingRepository = deviceSettingRepository;
            this.deviceSettingRepository.UnitOfWork = unitOfWork;
        }

        public void Create(DTOs.LabInstrument dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("dto");
            }

            var labInstrument = dto.ToData();

            //Save the record to database
            unitOfWork.ProcessWithTransaction(() => 
                {
                    labInstrumentRepository.Add(labInstrument);
                });
        }

        public LabInstrument Get(string instrumentId)
        {
            return this.labInstrumentRepository.GetQuery().Where(s => s.RecordId == instrumentId).FirstOrDefault().ToDto();
        }

        /// <summary>
        ///  Get labs or Instruments list according to parameters
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DTOs.LabInstrument> GetLabInstruments(string id, string userId)
        {
            List<DTOs.LabInstrument> labInstrumentDto = new List<DTOs.LabInstrument>();

            List<Data.Models.LabInstrument> labInstrumentData = this.labInstrumentRepository.GetQuery().Where(s => s.ParentId.Equals(id) && s.userId.Equals(userId)).ToList();

            foreach (var item in labInstrumentData)
            {
                labInstrumentDto.Add(item.ToDto());
            }

            return labInstrumentDto;

        }

        public bool BindInstrument(string instrumentId)
        {
            //var restServerIpV4 = this.globalRepository.GetByKey("rest", "serverIpV4");
            //var restPort = this.globalRepository.GetByKey("rest", "serverPort");
            var restServerAddress = this.globalRepository.GetByKey("rest", "serverRestAddress");

            if (restServerAddress == null || string.IsNullOrWhiteSpace(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_GLOBAL_SETTING_NOT_READY);

                return false;
            }

            if (!Helper.CanConnectServer(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_NETWORK_NOT_AVAILABLE);

                return false;
            }

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {
                try
                {
                    client.RequestUri = "bindInstru.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("instrumentId", instrumentId);

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    InstrumentSettingResult responseJson = Helper.ResponseJsonSerializer<InstrumentSettingResult>(rs);
                    if (responseJson.errorType.IsSameAs("success") || responseJson.errorCode.IsSameAs("success"))
                    {
                        var deviceModel = new DeviceSetting()
                        {
                            Category = "common",
                            OptionKey = "InstrumentId",
                            OptionValue = instrumentId
                        };

                        this.unitOfWork.ProcessWithTransaction(() =>
                        {
                            var data = deviceModel.ToData();

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

                            var bindInstrumentModel = this.deviceSettingRepository.GetByKey("common", "IsBindDevice");

                            if (bindInstrumentModel != null)
                            {
                                bindInstrumentModel.OptionValue = "True";
                                this.deviceSettingRepository.Update(bindInstrumentModel);
                            }
                        });

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<Exception>(Rs.VERITY_LAB_MANAGER_ERROR, ex);

                    return false;
                }
            }
            return false;
        }

        public bool UnBindInstrument(string instrumentId)
        {
            //var restServerIpV4 = this.globalRepository.GetByKey("rest", "serverIpV4");
            //var restPort = this.globalRepository.GetByKey("rest", "serverPort");
            var restServerAddress = this.globalRepository.GetByKey("rest", "serverRestAddress");

            if (restServerAddress == null || string.IsNullOrWhiteSpace(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_GLOBAL_SETTING_NOT_READY);

                return false;
            }

            if (!Helper.CanConnectServer(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_NETWORK_NOT_AVAILABLE);

                return false;
            }

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {
                try
                {
                    client.RequestUri = "unBind.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("instrumentId", instrumentId);

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    InstrumentSettingResult responseJson = Helper.ResponseJsonSerializer<InstrumentSettingResult>(rs);
                    if (responseJson.errorType.IsSameAs("success") || responseJson.errorCode.IsSameAs("success"))
                    {

                        this.unitOfWork.ProcessWithTransaction(() =>
                        {

                            this.deviceSettingRepository.Delete(s => s.Category == "common" && s.OptionKey == "InstrumentId");


                            var bindInstrumentModel = this.deviceSettingRepository.GetByKey("common", "IsBindDevice");

                            if (bindInstrumentModel != null)
                            {
                                bindInstrumentModel.OptionValue = "False";
                                this.deviceSettingRepository.Update(bindInstrumentModel);
                            }

                        });

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<Exception>(Rs.VERITY_LAB_MANAGER_ERROR, ex);

                    return false;
                }
            }
            return false;
        }

        public bool syncInstrumentInfo(string instrumentId)
        {
            //var restServerIpV4 = this.globalRepository.GetByKey("rest", "serverIpV4");
            //var restPort = this.globalRepository.GetByKey("rest", "serverPort");
            var restServerAddress = this.globalRepository.GetByKey("rest", "serverRestAddress");

            if (restServerAddress == null || string.IsNullOrWhiteSpace(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_GLOBAL_SETTING_NOT_READY);

                return false;
            }

            if (!Helper.CanConnectServer(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_NETWORK_NOT_AVAILABLE);

                return false;
            }

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {
                try
                {
                    client.RequestUri = "syncInstruInfo.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("instrumentId", instrumentId);

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    InstrumentSettingResult responseJson = Helper.ResponseJsonSerializer<InstrumentSettingResult>(rs);
                    if (responseJson.errorType.IsSameAs("success") || responseJson.errorCode.IsSameAs("success"))
                    {
                        unitOfWork.ProcessWithTransaction(() =>
                        {

                            var instrument = this.labInstrumentRepository.GetQuery(s => s.RecordId == instrumentId).FirstOrDefault();

                            if (instrument != null)
                            {
                                instrument.cancelAfterTime = responseJson.instrument.cancelAfterTime;
                                instrument.cancelPreTime = responseJson.instrument.cancelPreTime;
                                instrument.cancelWithoutActionTime = responseJson.instrument.cancelWithoutActionTime;
                                instrument.loginPreTime = responseJson.instrument.loginPreTime;
                            }
                            this.labInstrumentRepository.Update(instrument);

                        });

                        return true;
                    }
                    
                }
                catch (Exception ex)
                {
                    logger.Error<Exception>(Rs.VERITY_LAB_MANAGER_ERROR, ex);

                    return false;
                }
            }
            return false;
        }
    }
}
