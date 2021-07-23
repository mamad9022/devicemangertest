using Sepid.DeviceManagerTest.Common.Enum;
using System;

namespace Sepid.DeviceManagerTest.Common.Dto
{
    public class DeviceInfo
    {
        public string Ip { get; set; }
        public string Serial { get; set; }
        public UInt16 Port { get; set; }
        public SdkType SdkType { get; set; }
        public string Name { get; set; }
    }
}