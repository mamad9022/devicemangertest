using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Sepid.DeviceManagerTest.Api.Installer;
using Sepid.DeviceManagerTest.Api.Middleware;
using Sepid.DeviceManagerTest.Application;
using Sepid.DeviceManagerTest.Application.Common.Environment;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.DeviceServices;
using Sepid.DeviceManagerTest.Persistence;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using ZK.Common;

namespace Sepid.DeviceManagerTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesAssembly(Configuration);

            services.AddApplication();

            //services.AddIdentity(Configuration);

            services.AddPersistence(Configuration);
            services.AddScoped<IZksdk, Zksdk>();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env
            , IApplicationBootstrapper applicationBootstrapper
            , 
            IInitializeDeviceToServer initializeDevice,
            //ISeedPermissionService permissionService,
            ITransferService transferService, IDeviceServices deviceServices,
            IDataCollectorService dataCollectorService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            #region Initialize Sdks

            //foreach (var init in initializedServices)
            //    init.SdkInitialize();

            #endregion Initialize Sdks

            applicationBootstrapper.Initial();

            #region Folder

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Documents)),
                RequestPath = ApplicationStaticPath.Clients.Document
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Others)),
                RequestPath = ApplicationStaticPath.Clients.Other
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, ApplicationStaticPath.Images)),
                RequestPath = ApplicationStaticPath.Clients.Image
            });

            #endregion Folder

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseMultiLingual();
            app.UseMiddleware<ApplicationMetaMiddleware>();
            if (env.IsProduction())
            {
                app.UseMiddleware<LicenseMiddleware>();
            }


            #region hangfire

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hang-fire");

            #endregion hangfire

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("health", new HealthCheckOptions()
                { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

                endpoints.MapControllers();
            });

            #region backgroundJob


            BackgroundJob.Schedule(() => initializeDevice.Init(), TimeSpan.FromSeconds(10));
            BackgroundJob.Schedule(() => initializeDevice.InitialMatchOnServer(), TimeSpan.FromSeconds(30));
            RecurringJob.AddOrUpdate("RetryFailedTransfer", () => transferService.Retry(), "*/10 * * * *");
            RecurringJob.AddOrUpdate("LogCount", () => deviceServices.LogCount(), "*/30 * * * *");
            RecurringJob.AddOrUpdate("ClearLog", () => deviceServices.AutoClearLog(), "*/10 * * * *");
            RecurringJob.AddOrUpdate("AutoClearHangFireJob", () => deviceServices.AutoClearHangFireJob(), Cron.Hourly);
            RecurringJob.AddOrUpdate("EvacuationLog", () => initializeDevice.EvacuationNonVitalDevice(), "*/30 * * * * *");
            RecurringJob.AddOrUpdate("EvacuationVitalLog", () => initializeDevice.EvacuationVitalDevice(), "*/3 * * * * *");
            #endregion

            app.UseSwagger();

            await dataCollectorService.CollectData();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Device Manager Api V1");
            });

            //await permissionService.SeedPermission();
        }
    }
}