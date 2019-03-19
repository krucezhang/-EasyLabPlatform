using System;
using EasyLab.Server.DTOs;

namespace EasyLab.Server.Business
{
    static class Constants
    {
        /// <summary>
        /// The accurary of date time in ticks.
        /// </summary>
        public const int DateTimeAccuracy = 1;

        public static Guid SendToServerMachineId = new Guid("00000000-0000-0000-0000-000000000000");		
    }
}
