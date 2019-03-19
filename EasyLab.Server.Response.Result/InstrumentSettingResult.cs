using EasyLab.Server.DTOs;
using System.Collections.Generic;

namespace EasyLab.Server.Response.Result
{
    public class InstrumentSettingResult
    {
        public string errorCode { get; set; }

        public string errorType { get; set; }

        public InstrumentInfoResult instrument { get; set; }
    }

    public class InstrumentInfoResult
    {
        public int cancelPreTime { get; set; }

        public int cancelWithoutActionTime { get; set; }

        public int cancelAfterTime { get; set; }

        public int loginPreTime { get; set; }
    }
}
