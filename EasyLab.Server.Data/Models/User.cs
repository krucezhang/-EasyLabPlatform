/*************************************************************
* ?2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;

namespace EasyLab.Server.Data.Models
{
    public partial class User
    {
        public System.Guid UserId { get; set; }
        public string UserInfoId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string InstrumentId { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string UserType { get; set; }
        public System.DateTime LoginDateTime { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public string Comment { get; set; }
    }
}
