using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EasyLab.Server.Data.Models.Mapping;

namespace EasyLab.Server.Data.Models
{
    public partial class TestConnectionContext : DbContext
    {
        static TestConnectionContext()
        {
            Database.SetInitializer<TestConnectionContext>(null);
        }

        public TestConnectionContext()
            : base("Name=TestConnectionContext")
        {
        }

        public DbSet<LabAttribute> LablAttributes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GlobalSetting> GlobalSettings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<DeviceSetting> DeviceSettings { get; set; }
        public DbSet<LabInstrument> LabInstruments { get; set; }
        public DbSet<ReserveQueue> ReserveQueue { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new LabAttributeMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new GlobalSettingMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new ApplicationMap());
            modelBuilder.Configurations.Add(new AuditLogMap());
            modelBuilder.Configurations.Add(new MachineMap());
            modelBuilder.Configurations.Add(new DeviceSettingMap());
            modelBuilder.Configurations.Add(new LabInstrumentMap());
            modelBuilder.Configurations.Add(new ReserveQueueMap());
        }
    }
}
