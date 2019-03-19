/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Business.Validators;
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
    public class UserAppSvc : IUserAppSvc
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Data.Models.User> userRepository;
        private IRepository<Data.Models.GlobalSetting> globalRepository;
        private IRepository<Data.Models.LabInstrument> LabInstrumentRepository;
        private IRepository<Data.Models.DeviceSetting> deviceSettingRepository;

        protected const string EasyLab_Server_API_Address = "easylab_server_api_address";

        #region Static Members

        protected static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        public UserAppSvc(IUnitOfWork unitOfWork
            , IRepository<Data.Models.User> userRepository
            , IRepository<Data.Models.GlobalSetting> globalRepository
            , IRepository<Data.Models.LabInstrument> LabInstrumentRepository
            , IRepository<Data.Models.DeviceSetting> deviceSettingRepository)
        {
            this.unitOfWork = unitOfWork;

            this.userRepository = userRepository;
            this.userRepository.UnitOfWork = unitOfWork;

            this.globalRepository = globalRepository;
            this.globalRepository.UnitOfWork = unitOfWork;

            this.LabInstrumentRepository = LabInstrumentRepository;
            this.LabInstrumentRepository.UnitOfWork = unitOfWork;

            this.deviceSettingRepository = deviceSettingRepository;
            this.deviceSettingRepository.UnitOfWork = unitOfWork;
        }

        #region Query User

        public User Get(Guid id)
        {
            var user = userRepository.GetQuery(o => o.UserId == id).FirstOrDefault().ToDto();

            return user;
        }

        public IEnumerable<User> Get()
        {
            var user = userRepository.GetQuery();

            var dtos = new List<User>();

            foreach (var item in user.ToList())
            {
                var account = item.ToDto();
                dtos.Add(account);
            }
            return dtos;
        }

        public string checkUserLoginAuthority(string userInfoId)
        {
            var restServerAddress = this.globalRepository.GetByKey("rest", "serverRestAddress");

            if (restServerAddress == null || string.IsNullOrWhiteSpace(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_GLOBAL_SETTING_NOT_READY);

                return Resources.Rs.MESSAGE_LOGIN_FALSE;
            }
            var InstrumentId = GetStationIdFromDeviceSettings();
            if (InstrumentId == null)
            {
                return Resources.Rs.MESSAGE_LOGIN_FALSE;
            }
            if (!Helper.CanConnectServer(restServerAddress.OptionValue))
            {
                logger.Warn(Rs.LOG_MESSAGE_NETWORK_NOT_AVAILABLE);

                return Resources.Rs.MESSAGE_LOGIN_FALSE;
            }

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {
                try
                {

                    client.RequestUri = "checkAuthority.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("instrumentId", InstrumentId);
                    parameters.Add("userInfoId", userInfoId);

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    UsersResult responseJson = Helper.ResponseJsonSerializer<UsersResult>(rs);

                    if (responseJson.errorType.IsSameAs("success") || responseJson.errorCode.IsSameAs("success"))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return responseJson.errorCode;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<Exception>(Rs.VERITY_LAB_MANAGER_ERROR, ex);

                    return Resources.Rs.MESSAGE_LOGIN_FALSE;
                }
            }
        }

        public string GetUserInitListByInstrument(string instrumentId)
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

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {
                try
                {

                    client.RequestUri = "allUserInfo.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("instrumentId", instrumentId);

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    UsersResult responseJson = Helper.ResponseJsonSerializer<UsersResult>(rs);

                    if (responseJson.errorType.IsSameAs("success") || responseJson.errorCode.IsSameAs("success"))
                    {
                        UsersResult userResult = new UsersResult();

                        List<DTOs.User> allUsers = userResult.ToUser(responseJson);

                        var userDatas = this.userRepository.GetQuery().ToList();

                        //Get user list that was deleted list
                        var deleteUsers = new List<Data.Models.User>();

                        foreach (var userData in userDatas)
                        {
                            var isDelete = false;
                            foreach (var user in allUsers)
                            {
                                if (userData.UserInfoId == user.UserInfoId)
                                {
                                    isDelete = true;
                                }
                            }
                            if (!isDelete)
                            {
                                deleteUsers.Add(userData);
                            }
                        }

                        unitOfWork.ProcessWithTransaction(() =>
                        {
                            foreach (var dto in allUsers)
                            {
                                var user = dto.ToData();
                                var userData = this.userRepository.GetQuery().Where(s => s.UserInfoId.Equals(user.UserInfoId)).FirstOrDefault();
                                if (userData == null)
                                {
                                    user.UserId = IdentityGenerator.NewSequentialGuid();

                                    userRepository.Add(user);
                                }
                                else
                                {
                                    userData.InstrumentId = user.InstrumentId;
                                    userData.Password = new EasyLab.Server.Common.Extensions.RijndaelEnhanced().Encrypt(user.Password);
                                    userData.Email = IsNull(user.Email);
                                    userData.Phone = IsNull(user.Phone);
                                    userData.State = IsNull(user.State);
                                    userData.UserType = IsNull(user.UserType);
                                    userData.Type = IsNull(user.Type);
                                    userData.Comment = user.Comment;

                                    userRepository.Update(userData);
                                }
                            }
                            foreach (var user in deleteUsers)
                            {
                                this.userRepository.Delete(user);
                            }
                        });

                        return Rs.GETUSERLISTSUCCESS;
                    }
                    else
                    {
                        return Rs.GETUSERLISTERROR;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error<Exception>(Rs.VERITY_LAB_MANAGER_ERROR, ex);

                    return string.Empty;
                }
            }
        }

        private string IsNull(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value;
        }

        public string VerityManagerAccount(string username, string pwd)
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

            using (ServiceClient client = new ServiceClient(string.Format(ConfigurationManager.AppSettings[EasyLab_Server_API_Address], restServerAddress.OptionValue), logger))
            {

                try
                {
                    client.RequestUri = "labList.action?";

                    CookieCollection cookies = new CookieCollection();

                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("loginName", username);
                    parameters.Add("password", pwd);

                    HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                    string rs = Helper.ReadResponseStream(response);

                    LabResult responseJson = Helper.ResponseJsonSerializer<LabResult>(rs);

                    if (responseJson.errorType.IsSameAs("success"))
                    {
                        Data.Models.User userData = new Data.Models.User();

                        LabResult labResult = new LabResult();

                        List<LabInstrument> labList = labResult.ToLab(responseJson);

                        unitOfWork.ProcessWithTransaction(() =>
                            {
                                userData.UserId = IdentityGenerator.NewSequentialGuid();
                                userData.UserInfoId = responseJson.userInfoId;
                                userData.InstrumentId = string.Empty;
                                userData.LoginName = username;
                                userData.Password = new EasyLab.Server.Common.Extensions.RijndaelEnhanced().Encrypt(Helper.MD5Encrypt(pwd));
                                userData.Email = string.Empty;
                                userData.Phone = string.Empty;
                                userData.State = string.Empty;
                                userData.UserType = string.Empty;
                                userData.Type = string.Empty;
                                userData.LoginDateTime = DateTime.Now;
                                userData.CreateDateTime = DateTime.Now;
                                userData.Comment = string.Empty;

                                userRepository.Add(userData);

                                AddUpdateLabInstrumentInfo(client, labList, responseJson.userInfoId, username, pwd);
                            });
                    }

                    return responseJson.userInfoId;
                    
                }
                catch (Exception ex)
                {
                    logger.Error<Exception>(Rs.VERITY_LAB_MANAGER_ERROR, ex);

                    return string.Empty;
                }
                
            }
        }
        
        private void AddUpdateLabInstrumentInfo(ServiceClient client, List<LabInstrument> labList, string userid, string username, string pwd)
        {
            foreach (var item in labList)
            {
                var lab = item.ToData();

                //if it has repeat lab entities so do not save database.
                if (this.LabInstrumentRepository.FirstOrDefault(s => s.RecordId == item.RecordId && s.userId == userid) == null)
                {
                    lab.LabInstrumentId = IdentityGenerator.NewSequentialGuid();
                    lab.userId = userid;

                    this.LabInstrumentRepository.Add(lab);
                }

                //it will get instruments list 
                client.RequestUri = "instrumentList.action?";

                CookieCollection cookies = new CookieCollection();

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("labId", lab.RecordId);
                parameters.Add("loginName", username);
                parameters.Add("password", pwd);

                HttpWebResponse response = client.CreatePostHttpResponse(client.RequestUri, parameters, null, null, Encoding.UTF8, null);

                string rs = Helper.ReadResponseStream(response);

                InstrumentResult responseJson = Helper.ResponseJsonSerializer<InstrumentResult>(rs);

                if (responseJson.errorType.IsSameAs("success"))
                {
                    InstrumentResult instrumentResult = new InstrumentResult();

                    List<LabInstrument> instrumentList = instrumentResult.ToInstrument(responseJson);

                    foreach (var entity in instrumentList)
                    {
                        if (this.LabInstrumentRepository.FirstOrDefault(s => s.RecordId == entity.RecordId && s.userId == userid) == null)
                        {
                            var instrument = entity.ToData();

                            instrument.ParentId = lab.RecordId;
                            instrument.LabInstrumentId = IdentityGenerator.NewSequentialGuid();
                            instrument.userId = userid;

                            this.LabInstrumentRepository.Add(instrument);
                        }
                    }
                }
            }
        }

        #endregion
        
        public void Create(User dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("dto");
            }
            var validator = new CreateUserValidator();
            validator.ValidateAndThrowEasyLabException(dto);

            var user = dto.ToData();
            user.UserId = IdentityGenerator.NewSequentialGuid();
            user.LoginName = dto.LoginName;
            user.LoginDateTime = DateTime.UtcNow;
            user.CreateDateTime = DateTime.UtcNow;
            user.Password = SetPassword(dto.LoginName);
            user.Comment = string.Empty;

            unitOfWork.ProcessWithTransaction(() =>
                {
                    userRepository.Add(user);
                }
            );
        }

        #region Password

        private string SetPassword(string defaultValue)
        {
            return Helper.EncryptPassword(defaultValue);
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
        #endregion
    }
}
