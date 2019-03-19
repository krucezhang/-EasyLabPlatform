

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using EasyLab.Server.DTOs;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Resources;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.REST.Services.Models;


namespace EasyLab.Server.REST.Services.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix(Constants.UriRoot + "auditlogs")]
    public class AuditLogsController : ApiController
    {
        private IAuditLogAppSvc appSvc;

        public AuditLogsController(IAuditLogAppSvc appSvc)
        {
            this.appSvc = appSvc;
        }

        [GET("{id}", RouteName = "auditlogs_get_by_id")]
        public AuditLog Get(Guid id)
        {
            var data = appSvc.Get(id);

            if (data == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.NotFound, Rs.INVALID_RESOURCE_ID);
            }

            return data;
        }


        public HttpResponseMessage Post(AuditLog model)
        {
            if (model == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.BadRequest, Rs.MUST_NOT_BE_BLANK);
            }
            if (model.Log == null)
            {
                throw new EasyLabException(System.Net.HttpStatusCode.BadRequest, Rs.REQUIRED_FIELD_MISSING) { SubCode = "log" };
            }

            appSvc.Create(model);

            var response = Request.CreateResponse<AuditLog>(System.Net.HttpStatusCode.Created, model);
            string url = Url.Link("auditlog_get_by_id", new { @id = model.Log.Id, });
            response.Headers.Location = new Uri(url);

            return response;
        }
    }
}