using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class ApplicationMap : EntityTypeConfiguration<Application>
    {
        public ApplicationMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplicationId);

            // Properties
            this.Property(t => t.ApplicationId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Version)
                .HasMaxLength(15);

            this.Property(t => t.DBVersion)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Applications");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.DBVersion).HasColumnName("DBVersion");
        }
    }
}
