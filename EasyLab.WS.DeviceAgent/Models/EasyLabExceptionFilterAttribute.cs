using EasyLab.Server.Common.Errors;
using EasyLab.Server.Common.ExceptionExtensions;
using EasyLab.Server.Resources;
using NLog;
using System.Net.Http;
using System.Web.Http;

namespace EasyLab.WS.DeviceAgent.Models
{
    public class EasyLabExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is EasyLabException)
            {
                var easyLabError = (EasyLabException)actionExecutedContext.Exception;
                var httpError = new HttpError(easyLabError.Message);
                if (!string.IsNullOrEmpty(easyLabError.SubCode))
                {
                    httpError["subcode"] = easyLabError.SubCode;
                }
                httpError.Message = easyLabError.Message;

                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(easyLabError.Code, httpError);
            }
            else
            {
                var exception = actionExecutedContext.Exception;

                logger.Error(exception.GetAllExceptionMessage());

                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, Rs.INTERNAL_SERVICE_ERROR);
            }

            actionExecutedContext.Response.Headers.Add("easyLab-error-source", "EasyLabDeviceAgent");
        }
    }
}
