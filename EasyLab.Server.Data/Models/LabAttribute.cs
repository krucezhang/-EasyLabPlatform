using System;
using System.Collections.Generic;

namespace EasyLab.Server.Data.Models
{
    public partial class LabAttribute
    {
        public System.Guid LabId { get; set; }
        public System.Guid userId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialAttribute { get; set; }
    }
}
