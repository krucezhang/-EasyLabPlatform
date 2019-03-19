using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyLab.Server.AccessManager.Models
{
    public class GlobalSettings
    {
        public static string ActiveIdp { get; set; }
        public static string M1RESTServiceBaseAddress { get; set; }
        public static string FacilityName { get; set; }
        public static string ProductVersion { get; set; }
        public const string ProductId = "A1";
    }
}