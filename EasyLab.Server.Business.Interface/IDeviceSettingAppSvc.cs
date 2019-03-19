using EasyLab.Server.DTOs;

namespace EasyLab.Server.Business.Interface
{
    public interface IDeviceSettingAppSvc
    {
        void AddOrUpdate(DeviceSetting dto);

        DeviceSetting Get(string category, string optionKey);

        void DeleteDeviceConfig(DeviceSetting model);


        bool CanCanConnectServer();
    }
}
