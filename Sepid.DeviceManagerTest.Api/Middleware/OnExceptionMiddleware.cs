using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Sepid.DeviceManagerTest.Common.Helper;
using Serilog;

namespace Sepid.DeviceManagerTest.Api.Middleware
{
 
    public class OnExceptionMiddleware : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        
        public OnExceptionMiddleware(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var error = new ApiMessage();

            if (_env.IsDevelopment())
            {
                error.message = context.Exception.Message;
                error.Detail = context.Exception.StackTrace;
            }
            else
            {
                error.message = "A server error occurred";
                error.Detail = context.Exception.Message;
            }

            Log.Error(context.Exception, context.Exception.Message, context.Exception.StackTrace);

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
        }
    }
}