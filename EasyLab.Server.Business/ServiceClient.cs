using EasyLab.Server.Resources;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EasyLab.Server.Business
{
    class ServiceClient : IDisposable
    {
        private HttpClient client;

        private bool disposed;

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";  

        public string RequestUri
        {
            get;
            set;
        }

        public string BaseAddress
        {
            get;
            private set;
        }

        public Logger Logger { get; set; }

        public ServiceClient(string baseAddress)
            : this(baseAddress, null)
        {
        }

        public ServiceClient(string baseAddress, Logger logger)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.BaseAddress = BaseAddress;
            this.Logger = logger;
        }

        public HttpWebResponse CreatePostHttpResponse(string uri, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            string url = client.BaseAddress + uri;
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }

            HttpWebRequest request = null;
            //if send https request
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //if need POST data
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (var key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            return request.GetResponse() as HttpWebResponse;
        }

        public HttpWebResponse CreateGetHttpResponse(string uri, int? timeout, string userAgent, CookieCollection cookies)
        {
            string url = client.BaseAddress + uri;
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;  
        }

        public HttpResponseMessage Get()
        {
            return LogAndProcess(() => client.GetAsync(RequestUri).Result);
        }

        public HttpResponseMessage Post<T>(T value)
        {
            return LogAndProcess(() => client.PostAsync<T>(RequestUri, value, new JsonMediaTypeFormatter()).Result);
        }

        public HttpResponseMessage Put<T>(T value)
        {
            return LogAndProcess(() => client.PutAsync<T>(RequestUri, value, new JsonMediaTypeFormatter()).Result);
        }

        public HttpResponseMessage Delete()
        {
            return LogAndProcess(() => client.DeleteAsync(RequestUri).Result);
        }

        private HttpResponseMessage LogAndProcess(Func<HttpResponseMessage> func)
        {
            if (Logger != null)
                Logger.Info(Rs.LOG_SERVICE_CLIENT_REQUEST_CONTENT, this.BaseAddress + this.RequestUri);

            var response = func();

            if (Logger != null)
                Logger.Info<HttpResponseMessage>(Rs.LOG_SERVICE_CLIENT_RESPONSE_CONTENT, response);

            return response;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //Always accept
        } 

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }

            disposed = true;
        }
    }
}
