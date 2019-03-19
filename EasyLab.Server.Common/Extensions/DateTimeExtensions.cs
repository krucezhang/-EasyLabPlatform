/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;

namespace EasyLab.Server.Common.Extensions
{
    /// <summary>
    /// Extensions  methods for datetime
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert a datetime to UTC format string.(yyyy-MM-ddTHH:mm:ssZ)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUtcFormat(DateTime value)
        {
            return value.ToUniversalTime().ToString("s") + "Z";
        }
        /// <summary>
        /// Convert a nullable datetime to UTC format string, or empty if null.(yyyy-MM-ddTHH:mm:ssZ)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUtcFormat(DateTime? value)
        {
            if (value.HasValue)
            {
                return ToUtcFormat(value);
            }
            return string.Empty;
        }
    }
}
