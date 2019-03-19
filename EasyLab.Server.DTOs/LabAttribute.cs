/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/15/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.DTOs
{
    public class LabAttribute
    {
        public Guid LabId { get; set; }

        public Guid userId { get; set; }

        public string MaterialName { get; set; }

        public string MaterialAttribute { get; set; }
    }
}
