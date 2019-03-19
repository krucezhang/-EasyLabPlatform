using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class LabAttributeMap : EntityTypeConfiguration<LabAttribute>
    {
        public LabAttributeMap()
        {
            // Primary Key
            this.HasKey(t => t.LabId);

            // Properties
            this.Property(t => t.MaterialName)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.MaterialAttribute)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("LablAttribute");
            this.Property(t => t.LabId).HasColumnName("LabId");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.MaterialName).HasColumnName("MaterialName");
            this.Property(t => t.MaterialAttribute).HasColumnName("MaterialAttribute");
        }
    }
}
