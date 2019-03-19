using System.Data.Entity.ModelConfiguration;

namespace EasyLab.Server.Data.Models.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageId);

            // Properties
            this.Property(t => t.RecordId)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Tag)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("Messages");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.InstrumentId).HasColumnName("InstrumentId");
            this.Property(t => t.RecordId).HasColumnName("RecordId");
            this.Property(t => t.MessageType).HasColumnName("MessageType");
            this.Property(t => t.Processed).HasColumnName("Processed");
            this.Property(t => t.Failed).HasColumnName("Failed");
            this.Property(t => t.EntryDate).HasColumnName("EntryDate");
            this.Property(t => t.ProcessDate).HasColumnName("ProcessDate");
            this.Property(t => t.RetryCount).HasColumnName("RetryCount");
            this.Property(t => t.Tag).HasColumnName("Tag");
        }
    }
}
