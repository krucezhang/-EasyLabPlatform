/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            2/04/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;

namespace EasyLab.Server.Data.Models
{
    public partial class GlobalSetting
    {
        public string Category { get; set; }
        public string OptionKey { get; set; }
        public string OptionValue { get; set; }
    }
}
