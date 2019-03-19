using System;

namespace EasyLab.Server.Data.Models
{
    public class AuditLogSearchResult
    {
        public string StationName { get; set; }

        public DateTime CreateDate { get; set; }

        public string Sid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ResourceAction { get; set; }

        public Guid userId { get; set; }
    }
}
