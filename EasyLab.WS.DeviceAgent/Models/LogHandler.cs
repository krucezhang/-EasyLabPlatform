using NLog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyLab.WS.DeviceAgent.Models
{
    public class LogHandler : DelegatingHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var guiStr = Guid.NewGuid().ToString();
            string url = request.RequestUri.ToString();
            var timeBegin = DateTime.UtcNow;

            log.Trace(String.Format("{0} Begin to respond for url: {1}, at : {2}", guiStr, url, timeBegin));

            return base.SendAsync(request, cancellationToken).ContinueWith((task) =>
            {
                HttpResponseMessage response = task.Result as HttpResponseMessage;
                var timeEnd = DateTime.UtcNow;
                log.Trace(String.Format("{0} End to respond for url : {1} ,at : {2}, total time : {3}s", guiStr, url, timeEnd, (timeEnd - timeBegin).TotalSeconds));

                return response;
            });
        }
    }
}
