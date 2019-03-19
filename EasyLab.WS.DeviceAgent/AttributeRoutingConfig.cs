using System.Web.Http.SelfHost;
using AttributeRouting.Web.Http.SelfHost;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace EasyLab.WS.DeviceAgent
{
    public static class AttributeRoutingConfig
    {
        // Call this static method from a start up class in your applicaton (e.g.Program.cs)
        // Pass in the configuration you're using for your self-hosted Web API
        public static void RegisterRoutes(HttpSelfHostConfiguration config)
        {
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
            // To debug routes locally, you can log to Console.Out (or any other TextWriter) like so:
            //     config.Routes.Cast<HttpRoute>().LogTo(Console.Out);

            config.Routes.MapHttpAttributeRoutes();

            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.None;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
