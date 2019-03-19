using System.Web.Http;
using EasyLab.Server.REST.Services.App_Start;
using EasyLab.Server.REST.Services.Models;

namespace EasyLab.Server.REST.Services
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.Initialize();
            GlobalSettingConfig.Initialize(this.Context);
            FilterConfig.Register(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}