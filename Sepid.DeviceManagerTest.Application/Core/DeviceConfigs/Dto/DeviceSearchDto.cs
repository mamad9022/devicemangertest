using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Dto
{
    public class DeviceSearchDto
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Serial { get; set; }
        public SdkType SdkType { get; set; }
        public string Name { get; set; }
    }
}