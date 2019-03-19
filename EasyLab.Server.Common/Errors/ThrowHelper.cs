/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/3/2015
* Revision:        0.1       Draft
*                  
************************************************************/
      
using System;
using System.ComponentModel;

namespace EasyLab.Server.Common.Errors
{
    /// <summary>
    /// Helper class to throw exceptions.
    /// </summary>
   public static class ThrowHelper
    {
       /// <summary>
        /// Throw the ArgumentNullException with an argument name.
       /// </summary>
        /// <param name="argumentName">argument name</param>
       public static void ThrowArgumentNullException(string argumentName)
       {
           throw new ArgumentNullException(argumentName);
       }
       /// <summary>
       /// Throw the ArgumentNullException with an argument name if the argument parameter is null.
       /// </summary>
       /// <param name="argument">argument to check null</param>
       /// <param name="argumentName">argument name</param>
       public static void ThrowArgumentNullExceptionIfNull(object argument, string argumentName)
       {
           if (null == argument)
           {
               throw new ArgumentNullException(argumentName);
           }
       }
       
       /// <summary>
       /// Throw the ArgumentException with resource id.
       /// </summary>
       /// <param name="message">message</param>
       public static void ThrowArgumentException(string message)
       {
           throw new ArgumentException(message);
       }
       /// <summary>
       /// Throw the ArgumentException with an argument name if the string argument is null or empty.
       /// </summary>
       /// <param name="argument">string argument to check null or empty</param>
       /// <param name="argumentName">argument name</param>
       public static void ThrowArgumentExceptionIfEmpty(string argument, string argumentName)
       {

           if (string.IsNullOrEmpty(argument))
           {
               throw new ArgumentException(argumentName);
           }
       }
       /// <summary>
       /// hrow the ArgumentException with an argument name and exception message.
       /// </summary>
       /// <param name="argument">argument name</param>
       /// <param name="message">exception message</param>
       public static void ThrowArgumentException(string argument, string message)
       {
           throw new ArgumentException(message, argument);
       }
      
       /// <summary>
       /// Throw the InvalidEnumArgumentException if the enum value is not defined.
       /// </summary>
       /// <param name="enumType">enum type</param>
       /// <param name="value">enum value</param>
       public static void ThrowInvalidEnumArgumentExceptionIfNotDefined( Type enumType,object value)
       {
           if (!Enum.IsDefined(enumType, value))
           {
               throw new InvalidEnumArgumentException("value", (int)value, enumType);
           }
       }

       /// <summary>
       /// Throw the NotImplementedException with resource.
       /// </summary>
       /// <param name="message">exception emssage</param>
       public static void ThrowNotImplementedException(string message)
       {
           throw new NotImplementedException(message);
       }
    }
}
