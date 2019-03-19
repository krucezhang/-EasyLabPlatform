using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using EasyLab.WS.DeviceAgent.Models;
namespace EasyLab.WS.DeviceAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new DeviceAgentService() 
            };
            ServiceBase.Run(ServicesToRun);

            //DeviceAgentService service = new DeviceAgentService();

            //service.ProcessMessages(TimerTypes.None);

            //service.ProcessReserveQueue(TimerTypes.None);

            //service.CheckOnlineClient(TimerTypes.None);

            //service.ProcessUpdateQueue(TimerTypes.None);


#else

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new DeviceAgentService() 
            };
            ServiceBase.Run(ServicesToRun);
#endif

        }
    }
}
