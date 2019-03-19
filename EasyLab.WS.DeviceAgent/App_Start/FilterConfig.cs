
namespace EasyLab.WS.DeviceAgent.App_Start
{
    public class FilterConfig
    {
        public static void Register(System.Web.Http.HttpConfiguration config)
        {
            config.Filters.Add(new EasyLab.WS.DeviceAgent.Models.EasyLabExceptionFilterAttribute());
        }
    }
}
