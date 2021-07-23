using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sepid.DeviceManagerTest.Application.Common.Environment;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Service;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.LanguageService;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Redis;
using Sepid.DeviceManager.Application.Common.Service;

namespace Sepid.DeviceManagerTest.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

           
            services.AddScoped<ITransferService, RetryTransferService>();
            services.AddScoped<IDeviceServices, DeviceServices>();
            services.AddScoped<IDataCollectorService, DataCollectorService>();
            services.AddScoped<IUserAccessGroupService, UserAccessGroupService>();
            services.AddScoped<IGroupService, GroupService>();


            services.AddTransient<IInitializeDeviceToServer, InitializeDeviceToServer>();
            services.AddTransient<ILocalization, Localization>();
            services.AddTransient<IBatchOperationService, BatchOperationService>();
            services.AddTransient<IMediator, Mediator>();

            //services.AddSingleton<ITemplateService, TemplateService>();
            services.AddSingleton<IRedisServices, RedisService>();
            services.AddSingleton<ILanguageInfo, LanguageInfo>();
            services.AddSingleton<IApplicationBootstrapper, ApplicationBootstrapper>();
            services.AddSingleton<IRequestMeta, RequestMeta>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Api Behavior

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = new
                    {
                        message =
                            actionContext.ModelState.Values.SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage.ToString())
                                .FirstOrDefault()
                    };
                    return new BadRequestObjectResult(errors);
                };
            });

            #endregion Api Behavior

            return services;
        }
    }
}