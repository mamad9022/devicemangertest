using System;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Dto
{
    public class DeviceListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string ModelName { get; set; }
        public string Image { get; set; }
        public bool IsConnected { get; set; }
        public DateTime? LastLogRetrieve { get; set; }
        public bool ActiveImage { get; set; }
        public bool IsLock { get; set; }
        public long? CurrentLogCount { get; set; }
        public int TotalLog { get; set; }
        public double Percentage { get; set; }
        public SdkType SdkType { get; set; }
    }
}