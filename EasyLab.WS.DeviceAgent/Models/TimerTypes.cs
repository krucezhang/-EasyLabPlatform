using System;
using System.ComponentModel;

namespace EasyLab.WS.DeviceAgent.Models
{
    [Flags]
    internal enum TimerTypes
    {
        None = 0x0,

        Normal = 0x1,

        Retry = 0x2,

        [Description("000100")]
        TimeStamp = 0x4,

        [Description("001000")]
        ReserveQueue = 0x8, 

        [Description("010000")]
        OnlineTimer = 0x10,

        [Description("100000")]
        UpdateQueueTimer = 0x20,

        [Description("111111")]
        All = 0x3f
    }
}
