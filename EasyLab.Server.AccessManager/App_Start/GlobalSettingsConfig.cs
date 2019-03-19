using EasyLab.Server.AccessManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EasyLab.Server.AccessManager.App_Start
{
    public class GlobalSettingsConfig
    {
        public static void Init()
        {
            string address = ConfigurationManager.AppSettings["EasyLabRESTService"].ToLower();

            GlobalSettings.M1RESTServiceBaseAddress = address;
        }
    }
}