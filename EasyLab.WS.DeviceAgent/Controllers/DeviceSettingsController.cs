/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            5/06/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using AttributeRouting;
using AttributeRouting.Web.Http;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.DTOs;
using EasyLab.Server.Resources;
using EasyLab.WS.DeviceAgent.Models;
using System.Net;
using System.Web.Http;
using System.Net.Http;

namespace EasyLab.WS.DeviceAgent.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix(Constants.UriRoot + "devicesettings")]
    public class DeviceSettingsController : ApiController
    {
        private IDeviceSettingAppSvc deviceSettingSvc;

        public DeviceSettingsController()
        {
            using (var resolver = new DependencyResolver())
            {
                this.deviceSettingSvc = resolver.Resolve<IDeviceSettingAppSvc>();
            }
        }

        public DeviceSetting Get([FromUri] string category, [FromUri] string optionKey)
        {
            var data = deviceSettingSvc.Get(category, optionKey);

            if (data == null)
            {
                throw new EasyLabException(HttpStatusCode.NotFound, Rs.INVALID_RESOURCE_ID);
            }

            return data;
        }
        [GET("netIsValidate")]
        [AcceptVerbs("GET")]
        public bool CanCanConnectServer()
        {
            return deviceSettingSvc.CanCanConnectServer();
        }

        [DELETE("special/config")]
        [AcceptVerbs("DELETE")]
        public void DeleteDeviceConfig([FromUri] string category, [FromUri] string optionKey)
        {
            var data = deviceSettingSvc.Get(category, optionKey);

            if (data == null)
            {
                throw new EasyLabException(HttpStatusCode.NotFound, Rs.INVALID_RESOURCE_ID);
            }

            deviceSettingSvc.DeleteDeviceConfig(data);
        }

        public HttpResponseMessage Post(DeviceSetting model)
        {
            if (model == null)
            {
                throw new EasyLabException(HttpStatusCode.BadRequest, Rs.MUST_NOT_BE_BLANK);
            }

            deviceSettingSvc.AddOrUpdate(model);

            var response = Request.CreateResponse<DeviceSetting>(HttpStatusCode.Created, model);

            return response;
        }
    }
}
