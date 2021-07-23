using Sepid.DeviceManagerTest.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Models
{
    public class Device
    {

        public Device()
        {
            DeviceInGroups = new HashSet<DeviceInGroup>();
            TransferLogs = new HashSet<TransferLog>();
        }

        public int Id { get; set; }

        public long? LastRetrievedLogId { get; set; }
        public long? DeviceModelId { get; set; }
        public long? CurrentLogCount { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public string ServerIp { get; set; }
        public int? ServerPort { get; set; }
        public int Port { get; set; }
        public string Gateway { get; set; }
        public string SubnetMask { get; set; }

        public bool IsVital { get; set; }
        public bool UseDhcp { get; set; }
        public bool IsDeviceToServer { get; set; }
        public bool ActiveImage { get; set; }
        public bool IsMatchOnServer { get; set; }
        public bool IsConnected { get; set; }
        public bool IsLock { get; set; }
        public bool IsActive { get; set; }

        public EntranceMode EntranceMode { get; set; }

        public DateTime SyncLogStartDate { get; set; }
        public DateTime? LastLogRetrieve { get; set; }
        public DateTime CreateDateTime { get; set; }

        public virtual DeviceModel DeviceModel { get; set; }
        public ICollection<DeviceInGroup> DeviceInGroups { get; }
        public ICollection<TransferLog> TransferLogs { get; }

    }
}