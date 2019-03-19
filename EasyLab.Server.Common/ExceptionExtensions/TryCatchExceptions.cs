/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/9/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.Common.ExceptionExtensions
{
    public static class TryCatchExceptions
    {
        /// <summary>
        /// Catch the exception message, Do not pass parameter.
        /// </summary>
        /// <param name="func">function name</param>
        /// <returns>exception inner message</returns>
        public static string TryCatch(Action func)
        {
            string runMsg = string.Empty;
            try
            {
                func();
            }
            catch (Exception ex)
            {
                runMsg = ExceptionExtension.GetInnerException(ex);                
            }

            return runMsg;
        }
        /// <summary>
        /// Catch the exception inner message, Pass one parameter.
        /// </summary>
        /// <typeparam name="T">parameter type</typeparam>
        /// <param name="func">function name</param>
        /// <param name="param">function value</param>
        /// <returns>exception inner message</returns>
        public static string TryCatch<T>(Action<T> func, T param)
        {
            string runMsg = string.Empty;

            try
            {
                func(param);
            }
            catch (Exception ex)
            {
                runMsg = ExceptionExtension.GetInnerException(ex);
            }
            return runMsg;
        }
        /// <summary>
        /// Catch the Exception message
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="func"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public static string  TryCatch<T1, T2>(Action<T1, T2> func, T1 param1, T2 param2)
        {
            string runMsg = string.Empty;

            try
            {
                func(param1, param2);
            }
            catch (Exception ex)
            {
                runMsg = ExceptionExtension.GetInnerException(ex);
            }
            return runMsg;
        }
        /*
        public static string TryCatch(Func<TResult> func)
        {
            string runMsg = string.Empty;

            try
            {
                func();
            }
            catch (Exception ex)
            {
                runMsg = ExceptionExtension.GetInnerException(ex);
            }

            return runMsg;
        } */
    }
}
