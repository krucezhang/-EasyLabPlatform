using System;
using System.Collections.Generic;

namespace EasyLab.Server.Data.Models
{
    public partial class Machine
    {
        public Machine()
        {
            this.Messages = new List<Message>();
        }

        public System.Guid MachineId { get; set; }
        public string InstrumentId { get; set; }
        public string ComputerName { get; set; }
        public string IpV4Address { get; set; }
        public Nullable<System.DateTime> UpTimeStamp { get; set; }
        public bool InSession { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
