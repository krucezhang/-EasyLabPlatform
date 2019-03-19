using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.Response.Result
{
    public class AuditLogsResult
    {
        public string errorCode { get; set; }

        public string errorType { get; set; }

        public List<LogsResult> auditLog { get; set; }
    }
}
