using System;

namespace EasyLab.Server.DTOs
{
    public class Message
    {
        public Guid Id { get; set; }
        public string InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentIpV4 { get; set; }
        public uint InstrumentPort { get; set; }
        public string RecordId { get; set; }
        public int MessageType { get; set; }
        public bool Processed { get; set; }
        public bool Failed { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ProcessDate { get; set; }
        public uint RetryCount { get; set; }
        public string Tag { get; set; }
    }
}
