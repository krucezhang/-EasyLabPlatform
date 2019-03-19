using AttributeRouting;
using AttributeRouting.Web.Http;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.DTOs;
using EasyLab.Server.Resources;
using EasyLab.WS.DeviceAgent.Models;
using System;
using System.Net.Http;
using System.Web.Http;

namespace EasyLab.WS.DeviceAgent.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix(Constants.UriRoot + "messages")]
    public class MessagesController : ApiController
    {
        private IDeviceMessageAppSvc appSvc;

        public MessagesController()
        {
            using (var resolver = new DependencyResolver())
            {
                this.appSvc = resolver.Resolve<IDeviceMessageAppSvc>();
            }
        }

        [GET("{id}", RouteName = "messages_get_by_id")]
        public Message Get(Guid id)
        {
            var data = appSvc.Get(id);

            if (data == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.NotFound, Rs.INVALID_RESOURCE_ID);

            }

            return data;
        }

        public HttpResponseMessage Post(Message model)
        {
            if (model == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.BadRequest, Rs.MUST_NOT_BE_BLANK);
            }

            appSvc.Create(model);

            var response = Request.CreateResponse<Message>(System.Net.HttpStatusCode.Created, model);

            string url = Url.Link("messages_get_by_id", new { @id = model.Id, });

            response.Headers.Location = new Uri(url);


            return response;
        }
    }
}
