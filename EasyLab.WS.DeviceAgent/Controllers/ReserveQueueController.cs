using AttributeRouting;
using AttributeRouting.Web.Http;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.DTOs;
using EasyLab.Server.Resources;
using EasyLab.WS.DeviceAgent.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace EasyLab.WS.DeviceAgent.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix(Constants.UriRoot + "reserves")]
    public class ReserveQueueController : ApiController
    {
        private IReserveQueueSvc appSvc;

        public ReserveQueueController()
        {
            using (var resolver = new DependencyResolver())
            {
                this.appSvc = resolver.Resolve<IReserveQueueSvc>();
            }
        }

        public IEnumerable<Queue> Get()
        {
            return appSvc.ReserveQueueList();
        }
        [GET("getprevioususer")]
        [AcceptVerbs("GET")]
        public ReserveQueue GetPreviousReserveUser()
        {
            return appSvc.GetPreviousReserveUser();
        }

        public ReserveQueue GetQueueById(string id)
        {
            return appSvc.Get(id);
        }
        [PUT("cancel/record/{pwd}/{username}")]
        [AcceptVerbs("PUT")]
        public ReserveQueue AdminCancelReserveRecord(ReserveQueue reserve, string pwd, string username)
        {
            return appSvc.AdminCancelReserveRecord(reserve, pwd, username);
        }

        [GET("getinituserlist")]
        [AcceptVerbs("GET")]
        public string GetReserveQueueInitList([FromUri] string instrumentId)
        {
            if (string.IsNullOrWhiteSpace(instrumentId))
            {
                throw new EasyLabException(System.Net.HttpStatusCode.NotFound, EasyLabRs.INVALID_RESOURCE_ID);
            }

            return appSvc.AddOrUpdateReserveQueue(instrumentId);
        }

        public HttpResponseMessage Post(ReserveQueue model)
        {
            if (model == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.BadRequest, Rs.MUST_NOT_BE_BLANK);
            }

            ReserveQueue reserveQueueDto = appSvc.CreateOrUpdate(model);

            var response = Request.CreateResponse<ReserveQueue>(System.Net.HttpStatusCode.Created, reserveQueueDto);

            return response;
        }
    }
}
