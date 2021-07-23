using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Persistence.Context;

namespace Sepid.DeviceManagerTest.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DeviceManagerContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DeviceManagerContext"));

            }, ServiceLifetime.Transient);

            services.AddTransient<IDeviceManagerContext>(provider => provider.GetService<DeviceManagerContext>());

            return services;
        }
    }
}