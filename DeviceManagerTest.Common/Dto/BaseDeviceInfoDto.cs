using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Common.Dto
{
    public class BaseDeviceInfoDto
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public string Serial { get; set; }
        public string ServerIp { get; set; }
        public int? ServerPort { get; set; }
        public bool IsDeviceToServer { get; set; }
        public int MaxLogCount { get; set; }
        public int MachineNumber { get; set; }

        public DeviceModelTypes Code { get; set; }
    }
}