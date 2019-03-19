using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class MachineMap : EntityTypeConfiguration<Machine>
    {
        public MachineMap()
        {
            // Primary Key
            this.HasKey(t => t.MachineId);

            // Properties
            this.Property(t => t.InstrumentId)
                .IsRequired()
                .HasMaxLength(100);

            // Properties
            this.Property(t => t.ComputerName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IpV4Address)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Machines");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.InstrumentId).HasColumnName("InstrumentId");
            this.Property(t => t.ComputerName).HasColumnName("ComputerName");
            this.Property(t => t.IpV4Address).HasColumnName("IpV4Address");
            this.Property(t => t.UpTimeStamp).HasColumnName("UpTimeStamp");
            this.Property(t => t.InSession).HasColumnName("InSession");
        }
    }
}
