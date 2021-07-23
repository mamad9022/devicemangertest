using Sepid.DeviceManagerTest.Common.Enum;
using System;

namespace ZK.Helper
{
    public class DeviceConnectionInfo
    {
        public string Serial { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public DateTime? LastLogItem { get; set; }
        public uint? LastLogId { get; set; }
        public DeviceModelTypes Code { get; set; }
    }
}