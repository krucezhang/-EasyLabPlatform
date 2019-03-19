using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class DeviceSettingMap : EntityTypeConfiguration<DeviceSetting>
    {
        public DeviceSettingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Category, t.OptionKey });

            // Properties
            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OptionKey)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("DeviceSettings");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.OptionKey).HasColumnName("OptionKey");
            this.Property(t => t.OptionValue).HasColumnName("OptionValue");
        }
    }
}
