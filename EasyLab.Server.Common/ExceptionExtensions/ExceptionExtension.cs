/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/3/2015
* Revision:        0.1       Draft
*                  
************************************************************/

using System;

namespace EasyLab.Server.Common.ExceptionExtensions
{
    public static class ExceptionExtension
    {
        /// <summary>
        /// Get the message in the exception and its inner exception
        /// </summary>
        /// <param name="ex">exception</param>
        /// <returns></returns>
        public static string GetAllExceptionMessage(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            var sb = new System.Text.StringBuilder();
            if (!string.IsNullOrEmpty(ex.Message))
            {
                sb.AppendLine(ex.Message);
            }
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                sb.AppendLine(ex.StackTrace);
            }
            if (ex.InnerException != null)
            {
                sb.AppendLine();
                sb.Append(GetAllExceptionMessage(ex.InnerException));
            }

            return sb.ToString();
        }
        /// <summary>
        /// Get the inner message for Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetInnerException(this Exception ex)
        {
            string innerException = string.Empty;

            while (ex != null)
            {
                innerException = ex.Message;
                ex = ex.InnerException;
            }
            return innerException;
        }
    }
}
