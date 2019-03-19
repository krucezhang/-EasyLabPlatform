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
    [RoutePrefix(Constants.UriRoot + "labinstruments")]
    public class LabInstrumentsController : ApiController
    {
        private ILabInstrumentAppSvc appSvc;

        public LabInstrumentsController()
        {
            using (var resolver = new DependencyResolver())
            {
                this.appSvc = resolver.Resolve<ILabInstrumentAppSvc>();
            }
        }

        public LabInstrument Get([FromUri] string instrumentId)
        {
            return this.appSvc.Get(instrumentId);
        }

        [GET("bind")]
        [AcceptVerbs("GET")]
        public bool BindInstrument([FromUri] string instrumentId)
        {
            return this.appSvc.BindInstrument(instrumentId);
        }

        [GET("unbind")]
        [AcceptVerbs("GET")]
        public bool UnBindInstrument([FromUri] string instrumentId)
        {
            return this.appSvc.UnBindInstrument(instrumentId);
        }

        [GET("labs")]
        [AcceptVerbs("GET")]
        public IEnumerable<LabInstrument> GetLabs([FromUri] string id, [FromUri] string userid)
        {
            return this.appSvc.GetLabInstruments(id, userid);
        }

        [GET("instruments")]
        [AcceptVerbs("GET")]
        public IEnumerable<LabInstrument> GetInstruments([FromUri] string id, [FromUri] string userid)
        {
            return this.appSvc.GetLabInstruments(id, userid);
        }
    }
}
