using System;
using System.Collections.Generic;

namespace EasyLab.Server.Data.Models
{
    public partial class Message
    {
        public System.Guid MessageId { get; set; }
        public System.Guid MachineId { get; set; }
        public string InstrumentId { get; set; }
        public string RecordId { get; set; }
        public short MessageType { get; set; }
        public bool Processed { get; set; }
        public bool Failed { get; set; }
        public System.DateTime EntryDate { get; set; }
        public Nullable<System.DateTime> ProcessDate { get; set; }
        public int RetryCount { get; set; }
        public string Tag { get; set; }

        public virtual Machine Machine { get; set; }
    }
}
