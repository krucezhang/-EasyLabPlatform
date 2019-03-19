/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System.Collections.Generic;
using System.Globalization;

namespace EasyLab.Server.Common.Extensions
{
    /// <summary>
    /// Extensions method for validator
    /// </summary>
    public static class ValidationExtension
    {
        /// <summary>
        /// Convert a string value to a camel case
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (!char.IsUpper(value[0]))
            {
                return value;
            }
            string text = char.ToLower(value[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
            if (value.Length > 1)
            {
                text += value.Substring(0, 1);
            }
            return text;
        }
        /// <summary>
        /// Convert a list of string values to camel case.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToCamelCase(this IEnumerable<string> values)
        {
            foreach (var item in values)
            {
                yield return item.ToCamelCase();
            }
        }
        /// <summary>
        /// Compare whether two string are equal if ignoring case.
        /// </summary>
        /// <param name="strOne"></param>
        /// <param name="strTwo"></param>
        /// <returns>true if the value of the a parameter is equal to the value of the b parameter; otherwise, false</returns>
        public static bool EqualsIgnoreCase(this string strOne, string strTwo)
        {
            return string.Equals(strOne, strTwo, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
