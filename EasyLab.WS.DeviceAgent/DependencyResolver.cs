using EasyLab.Server.Data.Persistence;
using EasyLab.Server.Data.Persistence.Repositories;
using EasyLab.Server.Data.Models;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Business;
using Microsoft.Practices.Unity;
using System;
using System.Data.Entity;
using EasyLab.Server.Repository.Interface;

namespace EasyLab.WS.DeviceAgent
{
    public class DependencyResolver : IDisposable
    {
        private IUnityContainer container;

        public DependencyResolver()
        {
            container = new UnityContainer();

            container.RegisterType<DbContext, TestConnectionContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            container.RegisterType<IRepository<User>, UserRepository>();
            container.RegisterType<IRepository<LabAttribute>, LabAttributeRepository>();
            container.RegisterType<IRepository<GlobalSetting>, GlobalSettingRepository>();
            container.RegisterType<IRepository<Message>, MessageRepository>();
            container.RegisterType<IRepository<AuditLog>, AuditLogRepository>();
            container.RegisterType<IRepository<Application>, ApplicationRepository>();
            container.RegisterType<IRepository<Machine>, MachineRepository>();
            container.RegisterType<IRepository<DeviceSetting>, DeviceSettingRepository>();
            container.RegisterType<IRepository<LabInstrument>, LabInstrumentRepository>();
            container.RegisterType<IRepository<ReserveQueue>, ReserveQueueRepository>();

            container.RegisterType<IUserAppSvc, UserAppSvc>();
            container.RegisterType<ILabAttributeAppSvc, LabAttributeAppSvc>();
            container.RegisterType<IGlobalSettingAppSvc, GlobalSettingAppSvc>();
            container.RegisterType<IAuditLogAppSvc, AuditLogAppSvc>();
            container.RegisterType<IDeviceMessageAppSvc, DeviceMessageAppSvc>();
            container.RegisterType<IDeviceSettingAppSvc, DeviceSettingAppSvc>();
            container.RegisterType<ILabInstrumentAppSvc, LabInstrumentAppSvc>();
            container.RegisterType<IReserveQueueSvc, ReserveQueueSvc>();
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}
