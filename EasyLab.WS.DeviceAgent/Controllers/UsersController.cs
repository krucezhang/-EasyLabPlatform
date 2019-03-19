/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            3/01/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using AttributeRouting;
using AttributeRouting.Web.Http;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.DTOs;
using EasyLab.WS.DeviceAgent.Models;
using System.Collections.Generic;
using EasyLab.Server.Common.Errors;
using System.Web.Http;

namespace EasyLab.WS.DeviceAgent.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix(Constants.UriRoot + "users")]
    public class UsersController : ApiController
    {
        private IUserAppSvc appSvc;

        public UsersController()
        {
            using (var resolver = new DependencyResolver())
            {
                this.appSvc = resolver.Resolve<IUserAppSvc>();
            }
        }

        public IEnumerable<User> Get()
        {
            var data = appSvc.Get();

            foreach (var item in data)
            {
                item.Password = new EasyLab.Server.Common.Extensions.RijndaelEnhanced().Decrypt(item.Password);
            }

            return data;
        }

        [GET("checkauth")]
        [AcceptVerbs("GET")]
        public string checkUserLoginAuthority([FromUri] string userInfoId)
        {
            return appSvc.checkUserLoginAuthority(userInfoId);
        }

        [GET("getinituserlist")]
        [AcceptVerbs("GET")]
        public string GetUserListByInstrument([FromUri] string instrumentId)
        {
            if (string.IsNullOrWhiteSpace(instrumentId))
            {
                throw new EasyLabException(System.Net.HttpStatusCode.NotFound, EasyLabRs.INVALID_RESOURCE_ID);
            }

            return appSvc.GetUserInitListByInstrument(instrumentId);
        }

        [GET("validatemanager")]
        [AcceptVerbs("GET")]
        public string VerityLabManagerLogin([FromUri] string username, [FromUri] string password)
        {

            if (username == null || password == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.NotFound, EasyLabRs.INVALID_RESOURCE_ID);
            }


            return appSvc.VerityManagerAccount(username, password);
        }
    }
}
