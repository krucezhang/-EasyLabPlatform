/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/3/2015
* Revision:        0.1       Draft
*                  
************************************************************/

      
using System;
using System.Net;

namespace EasyLab.Server.Common.Errors
{
    /// <summary>
    /// Represents the custom error in M1.
    /// </summary>
    [Serializable]
    public class EasyLabException : System.Exception
    {
        private HttpStatusCode code;
        private string subCode;

        public EasyLabException():this(string.Empty,null)
        {
           
        }

        public EasyLabException(string message):this(message,null)
        {
        }

        public EasyLabException(string message, Exception innerException):base(message,innerException)
        {
            this.code = HttpStatusCode.InternalServerError;
        }

        public EasyLabException(HttpStatusCode code)
            : this(code, string.Empty, (Exception)null)
        {
        }

        public EasyLabException(HttpStatusCode code, string message):this(code,message,(Exception)null)
        {
        }

        public EasyLabException(HttpStatusCode code, string format, params object[] args)
            : this(code, string.Format(format, args), (Exception)null)
        {

        }

        public EasyLabException(HttpStatusCode code, string message, Exception innerException)
            : base(message, innerException)
        {
            this.code = code;
        }

        /// <summary>
        /// Get the error code.
        /// </summary>
        public HttpStatusCode Code { get { return code; } }
        /// <summary>
        /// Get or set the error subcode.
        /// </summary>
        public string SubCode
        {
            get { return subCode; }
            set { subCode = value; }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            base.GetObjectData(info, context);

            info.AddValue("code", this.code, typeof(HttpStatusCode));
            info.AddValue("subCode", this.subCode, typeof(string));
        }
    }
}
