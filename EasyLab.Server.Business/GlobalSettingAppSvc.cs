
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Data.Persistence;
using EasyLab.Server.Conversions;
using EasyLab.Server.Common.Extensions;
using EasyLab.Server.Repository.Interface;
using EasyLab.Server.Resources;
using System;
using NLog;
using System.Net;
using System.Collections.Generic;
using EasyLab.Server.Response.Result;
using System.Configuration;
using System.Text;

namespace EasyLab.Server.Business
{
    public class GlobalSettingAppSvc : IGlobalSettingAppSvc
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Data.Models.GlobalSetting> globalSettingRepository;

        #region Static Members

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion
        protected const string EasyLab_Server_API_Address = "easylab_server_api_address";
        private static string[,] protectedResources = { { "auth", "dmaPassword" } };

        public GlobalSettingAppSvc(IUnitOfWork unitOfWork,
            IRepository<Data.Models.GlobalSetting> globalSetting)
        {
            this.unitOfWork = unitOfWork;

            this.globalSettingRepository = globalSetting;
            this.globalSettingRepository.UnitOfWork = unitOfWork;
        }

        public DTOs.GlobalSetting Get(string category, string optionKey)
        {
            //if (IsProtectedResource(category, optionKey))
            //{
            //    throw new EasyLab.Server.Common.Errors.EasyLabException(System.Net.HttpStatusCode.Forbidden, Rs.REQUEST_FORBIDDEN);
            //}

            return this.globalSettingRepository.GetByKey(category, optionKey).ToDto();
        }


        public bool CheckOnlineClient(string instrumentId)
        {
            //var restServerIpV4 = this.globalRepository.GetByKey("rest", "serverIpV4");
            //var restPort = this.globalRepository.GetByKey("rest", "serverPort");
            var restServerAddress = this.globalSettingRepository.GetByKey("rest", "serverRestAddress");

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
                    client.RequestUri = "checkOnLine.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("instrumentId", instrumentId);

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    OnlineResult responseJson = Helper.ResponseJsonSerializer<OnlineResult>(rs);
                    if (responseJson.errorType.IsSameAs("success") || responseJson.errorCode.IsSameAs("success"))
                    {
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

        private bool IsProtectedResource(string category, string optionKey)
        {
            int rows = protectedResources.GetLength(0);

            for (int i = 0; i < rows; i++)
            {
                if (category.EqualsIgnoreCase(protectedResources[i, 0]) && optionKey.EqualsIgnoreCase(protectedResources[i, 1]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
