using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EasyLab.Server.REST.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.Clear();

            config.Routes.MapHttpAttributeRoutes();
        }
    }
}
