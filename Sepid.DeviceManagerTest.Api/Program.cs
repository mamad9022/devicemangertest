using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sepid.DeviceManagerTest.Application.Core.System;
using Sepid.DeviceManagerTest.Common.Options;
using Sepid.DeviceManagerTest.Persistence.Context;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                #region SeriLog


                var seriLogSetting = new SeriLogSetting();
                config.GetSection("SeriLogSetting").Bind(seriLogSetting);

                host = CreateHostBuilder(args).Build();

                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .MinimumLevel.Information()
                    .WriteTo.Seq(seriLogSetting.Address)
                    .CreateLogger();
                Log.Information(" SeriLog Initialized on {Address} ... ", seriLogSetting.Address);

                #endregion SeriLog

                #region Database

                var context = services.GetRequiredService<DeviceManagerContext>();
                await context.Database.MigrateAsync();

                var mediator = services.GetRequiredService<IMediator>();
                await mediator.Send(new SampleSeedDataCommand(), CancellationToken.None);

                #endregion Database

                await host.RunAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}