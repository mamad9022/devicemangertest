using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto;
using System;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Dto
{
    public class DeviceDto
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string Port { get; set; }
        public string Gateway { get; set; }
        public string SubnetMask { get; set; }

        public bool IsVital { get; set; }
        public bool UseDhcp { get; set; }
        public bool IsMatchOnServer { get; set; }
        public bool IsConnected { get; set; }
        public bool IsDeviceToServer { get; set; }
        public bool ActiveImage { get; set; }
        public bool IsLock { get; set; }

        public string SyncLogPeriod { get; set; }
        public DateTime SyncLogStartDate { get; set; }
        public DateTime? LastLogRetrieve { get; set; }

        public EntranceMode EntranceMode { get; set; }
        public DeviceModelDto DeviceModel { get; set; }
    }
}