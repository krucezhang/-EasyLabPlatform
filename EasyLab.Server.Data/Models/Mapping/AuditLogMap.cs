using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class AuditLogMap : EntityTypeConfiguration<AuditLog>
    {
        public AuditLogMap()
        {
            // Primary Key
            this.HasKey(t => t.AuditLogId);

            // Properties
            this.Property(t => t.ResourceType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ResourceValue)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.ResourceAction)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.InstrumentId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ApplicationId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.ResourceType2)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ResourceValue2)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("AuditLogs");
            this.Property(t => t.AuditLogId).HasColumnName("AuditLogId");
            this.Property(t => t.ResourceType).HasColumnName("ResourceType");
            this.Property(t => t.ResourceValue).HasColumnName("ResourceValue");
            this.Property(t => t.ResourceAction).HasColumnName("ResourceAction");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.InstrumentId).HasColumnName("InstrumentId");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ResourceType2).HasColumnName("ResourceType2");
            this.Property(t => t.ResourceValue2).HasColumnName("ResourceValue2");
        }
    }
}
