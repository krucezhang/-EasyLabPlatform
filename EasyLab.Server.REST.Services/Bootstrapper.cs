using EasyLab.Server.Business;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.Data.Models;
using EasyLab.Server.Data.Persistence;
using EasyLab.Server.Data.Persistence.Repositories;
using EasyLab.Server.Repository.Interface;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web.Http;

namespace EasyLab.Server.REST.Services
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            var container = BuildUnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<DbContext, TestConnectionContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            container.RegisterType<IRepository<User>, UserRepository>();
            container.RegisterType<IRepository<LabAttribute>, LabAttributeRepository>();
            container.RegisterType<IRepository<AuditLog>, AuditLogRepository>();

            container.RegisterType<IUserAppSvc, UserAppSvc>();
            container.RegisterType<ILabAttributeAppSvc, LabAttributeAppSvc>();
            container.RegisterType<IAuditLogAppSvc, AuditLogAppSvc>();

            return container;
        }
    }
}