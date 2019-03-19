using System;
using System.Collections.Generic;

namespace EasyLab.Server.Data.Models
{
    public partial class AuditLog
    {
        public System.Guid AuditLogId { get; set; }
        public string ResourceType { get; set; }
        public string ResourceValue { get; set; }
        public string ResourceAction { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string InstrumentId { get; set; }
        public string ApplicationId { get; set; }
        public string UserId { get; set; }
        public string ResourceType2 { get; set; }
        public string ResourceValue2 { get; set; }

        public virtual User User { get; set; }
        public virtual Application Application { get; set; }
    }
}
