using EasyLab.Server.DTOs;
using System.Collections.Generic;

namespace EasyLab.Server.Response.Result
{
    public class OnlineResult
    {
        public string errorCode { get; set; }

        public string errorType { get; set; }

        public string instrumentId { get; set; }
    }
}
