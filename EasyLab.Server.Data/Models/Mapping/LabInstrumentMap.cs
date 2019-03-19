using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class LabInstrumentMap : EntityTypeConfiguration<LabInstrument>
    {
        public LabInstrumentMap()
        {
            //Primary Key
            this.HasKey(t => t.LabInstrumentId);

            // Properties
            this.Property(t => t.ParentId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.RecordId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.RecordName)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.userId)
                .IsRequired()
                .HasMaxLength(100);

            //Table & column Mappings
            this.ToTable("LabInstruments");
            this.Property(t => t.RecordId).HasColumnName("RecordId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.RecordName).HasColumnName("RecordName");
            this.Property(t => t.cancelAfterTime).HasColumnName("cancelAfterTime");
            this.Property(t => t.cancelPreTime).HasColumnName("cancelPreTime");
            this.Property(t => t.cancelWithoutActionTime).HasColumnName("cancelWithoutActionTime");
            this.Property(t => t.loginPreTime).HasColumnName("loginPreTime");
            this.Property(t => t.userId).HasColumnName("userId");
        }
    }
}
