using System.ComponentModel;
using System.ServiceProcess;

namespace EasyLab.WS.DeviceAgent
{
    [RunInstaller(true)]
    public partial class DeviceAgentInstaller : System.Configuration.Install.Installer
    {
        public DeviceAgentInstaller()
        {
            InitializeComponent();

            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            processInstaller.Username = null;
            processInstaller.Password = null;

            //serviceInstaller.ServiceName = DeviceAgentService.Id;
            //serviceInstaller.DisplayName = EasyLabRs.ServiceDisplayName;
            //serviceInstaller.Description = EasyLabRs.ServiceDescription;
            //serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "EasyLabRs";
            serviceInstaller.DisplayName = "EasyLabRs DIS";
            serviceInstaller.Description = "EasyLabRs DESC";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //serviceInstaller.DelayedAutoStart = true;

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);

        }
    }
}
