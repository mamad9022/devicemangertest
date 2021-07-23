using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Sepid.DeviceManagerTest.Client.RabbitMq;

namespace Sepid.DeviceManagerTest.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDeviceManager(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RabbitMqConnection>(configuration.GetSection("RabbitMqConnection"));

            services.TryAddSingleton<IRabbitMqConnection>
                (sp => sp.GetRequiredService<IOptions<RabbitMqConnection>>().Value);

            services.AddScoped<IDeviceSubscriber, DeviceSubscriber>();

            return services;
        }
    }
}