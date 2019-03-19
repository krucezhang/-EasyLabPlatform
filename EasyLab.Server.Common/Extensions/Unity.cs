
using System;
namespace EasyLab.Server.Common.Extensions
{
    public static class Unity
    {
        public static bool IsSameAs(this string left, string right)
        {
            return string.Compare(left, right, true) == 0;
        }

        public static DateTime StringToDate(string value)
        {
            DateTime dt = new DateTime();

            if (DateTime.TryParse(value, out dt))
            {
                return dt;
            }

            return dt;
        }

        public static int StringToInt(string value)
        {
            int iValue;

            if (Int32.TryParse(value, out iValue))
            {
                return iValue;
            }

            return 0;
        }

        public static bool DateTimeCompare(this DateTime left, DateTime right)
        {
            return DateTime.Compare(left, right) <= 0;
        }
    }
}
