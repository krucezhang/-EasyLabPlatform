using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class ReserveQueueMap : EntityTypeConfiguration<ReserveQueue>
    {
        public ReserveQueueMap()
        {
            // Primary Key
            this.HasKey(t => t.queueId);

            this.Property(t => t.reserveId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.startDate)
                .IsRequired();

            this.Property(t => t.endDate)
                .IsRequired();

            this.Property(t => t.loginDate)
                .IsRequired();

            this.Property(t => t.logoutDate)
                .IsRequired();

            this.Property(t => t.cancelReserve)
                .IsRequired();

            this.Property(t => t.autoCancelReserve)
                .IsRequired();

            this.Property(t => t.comment)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ReserveQueue");
            this.Property(t => t.queueId).HasColumnName("queueId");
            this.Property(t => t.reserveId).HasColumnName("reserveId");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.startDate).HasColumnName("startDate");
            this.Property(t => t.endDate).HasColumnName("endDate");
            this.Property(t => t.loginDate).HasColumnName("loginDate");
            this.Property(t => t.logoutDate).HasColumnName("logoutDate");
            this.Property(t => t.cancelReserve).HasColumnName("cancelReserve");
            this.Property(t => t.autoCancelReserve).HasColumnName("autoCancelReserve");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Sequence).HasColumnName("Sequence");
            this.Property(t => t.IsTemporary).HasColumnName("IsTemporary");
            this.Property(t => t.comment).HasColumnName("comment");
        }
    }
}
