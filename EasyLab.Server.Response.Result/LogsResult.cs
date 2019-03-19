using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.Response.Result
{
    public class LogsResult
    {
        public Guid auditLogId { get; set; }

        public string instrumentId { get; set; }

        public string applicationId { get; set; }

        public string userId { get; set; }

        public string createDate { get; set; }

        public string resourceAction { get; set; }

        public string resourceType { get; set; }

        public string resourceValue { get; set; }

        public string resourceType2 { get; set; }

        public string resourceValue2 { get; set; }
    }
}
