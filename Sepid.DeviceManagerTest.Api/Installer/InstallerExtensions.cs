using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sepid.DeviceManager.Api;
using System;
using System.Linq;

namespace Sepid.DeviceManagerTest.Api.Installer
{
    public static class InstallerExtensions
    {
        public static void InstallServicesAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installer = typeof(Startup).Assembly.ExportedTypes.Where(x =>
                    typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installer.ForEach(install => install.InstallServices(configuration, services));
        }
    }
}