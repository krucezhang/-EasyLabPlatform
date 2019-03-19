/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            3/06/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using AttributeRouting;
using AttributeRouting.Web.Http;
using EasyLab.Server.Business;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.DTOs;
using EasyLab.Server.Resources;
using EasyLab.WS.DeviceAgent.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EasyLab.WS.DeviceAgent.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix(Constants.UriRoot + "auditlogs")]
    public class AuditLogsController : ApiController
    {
        private IAuditLogAppSvc auditLogSvc;
        private IDeviceMessageAppSvc msgSvc;

        public AuditLogsController()
        {
            using (var resolver = new DependencyResolver())
            {
                this.auditLogSvc = resolver.Resolve<IAuditLogAppSvc>();
                this.msgSvc = resolver.Resolve<IDeviceMessageAppSvc>();
            }
        }

        public IEnumerable<AuditLog> Get([FromUri] SearchFilter filter)
        {
            return auditLogSvc.Get(filter);
        }

        [GET("{id}")]
        public AuditLog Get(Guid id)
        {
            var data = auditLogSvc.Get(id);

            if (data == null)
            {
                throw new EasyLabException(HttpStatusCode.NotFound, Rs.INVALID_RESOURCE_ID);
            }

            return data;
        }

        public HttpResponseMessage Post(AuditLog model)
        {
            if (model == null)
            {
                throw new EasyLabException(HttpStatusCode.BadRequest, Rs.MUST_NOT_BE_BLANK);
            }
            if (model.Log == null)
            {
                throw new EasyLabException(HttpStatusCode.BadRequest, Rs.REQUIRED_FIELD_MISSING) { SubCode = "log" };
            }

            auditLogSvc.Create(model);

            this.msgSvc.Create(new Message
                {
                    InstrumentId = model.Log.InstrumentId,
                    RecordId = model.Log.Id.ToString(),
                    MessageType = (int)MessageTypes.PostbackAuditLogs
                });

            var response = Request.CreateResponse<AuditLog>(HttpStatusCode.Created, model);

            return response;
        }
    }
}
