using System;
using System.Linq;
using System.Threading.Tasks;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Http;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Api.Middleware
{

    public class ApplicationMetaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRequestMeta _requestMeta;

        public ApplicationMetaMiddleware(RequestDelegate next, IRequestMeta requestMeta)
        {
            _next = next;
            _requestMeta = requestMeta;
        }


        public Task Invoke(HttpContext httpContext)
        {
            var userAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault();
            try
            {
              
                var userAgentInfo = DeviceDetector.GetInfoFromUserAgent(userAgent);
                _requestMeta.Ip = httpContext.Request.Headers["X-Forwarded-For"].ToString();
                // _requestMeta.Ip = httpContext.Connection.RemoteIpAddress.ToString();

                if (userAgentInfo.Match.BrowserFamily != "Unknown")
                {
                    _requestMeta.Browser = userAgentInfo.Match.Client.Name;
                    _requestMeta.Os = userAgentInfo.Match.Os.Name;
                    _requestMeta.Device = userAgentInfo.Match.DeviceType;
                    _requestMeta.UserAgent = string.IsNullOrWhiteSpace(userAgent)?"Application":userAgent;
                }
                else
                {
                    _requestMeta.Browser = "";
                    _requestMeta.Os = "";
                    _requestMeta.Device = "";
                    _requestMeta.UserAgent =  string.IsNullOrWhiteSpace(userAgent)?"Application":userAgent;
                }
            }
            catch (Exception)
            {
                _requestMeta.Browser = "";
                _requestMeta.Os = "";
                _requestMeta.Device = "";
                _requestMeta.UserAgent = "unknown";
            }

            return _next(httpContext);
        }
    }

}