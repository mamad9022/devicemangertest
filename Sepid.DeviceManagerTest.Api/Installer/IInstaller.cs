using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sepid.DeviceManagerTest.Api.Installer
{
    public interface IInstaller
    {
        void InstallServices(IConfiguration configuration, IServiceCollection services);
    }
}