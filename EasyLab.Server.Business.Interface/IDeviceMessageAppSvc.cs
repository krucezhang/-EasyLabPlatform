
namespace EasyLab.Server.Business.Interface
{
    public interface IDeviceMessageAppSvc : IMessageAppSvc
    {
        void Process();

        void Retry();
    }
}
