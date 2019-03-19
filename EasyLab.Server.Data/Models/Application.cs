using System;
using System.Collections.Generic;

namespace EasyLab.Server.Data.Models
{
    public partial class Application
    {
        public string ApplicationId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string DBVersion { get; set; }
    }
}
