using EasyLab.WS.DeviceAgent.Models;

namespace EasyLab.WS.DeviceAgent
{
    internal static class Unity
    {
        public static bool HasValue(this TimerTypes type, TimerTypes timerType)
        {
            return (type & timerType) == timerType;
        }
    }
}
