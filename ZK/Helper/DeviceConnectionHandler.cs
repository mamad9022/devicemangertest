using Sepid.DeviceManagerTest.Common.Dto.DeviceLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZK.Helper
{
    public class DeviceConnectionHandler
    {
        #region Singleton

        private static DeviceConnectionHandler _instance;

        public static DeviceConnectionHandler Instance => _instance ??= new DeviceConnectionHandler();

        #endregion Singleton

        public DeviceConnectionHandler()
        {
            DeviceConnection = new Dictionary<string, DeviceConnectionInfo>();
        }

        public IDictionary<string, DeviceConnectionInfo> DeviceConnection;

        public List<string> GetAllDevices() => DeviceConnection.Keys.ToList();

        public DeviceConnectionInfo GetDeviceInfo(string serial)
        {
            DeviceConnection.TryGetValue(serial, out var device);

            return device;
        }

        public void AddDevice(FilteredDeviceLogDto baseDeviceInfo)
        {
            DeviceConnection.TryAdd(baseDeviceInfo.Serial, new DeviceConnectionInfo
            {
                Ip = baseDeviceInfo.Ip,
                Serial = baseDeviceInfo.Serial,
                Port = baseDeviceInfo.Port,
                LastLogId = 0,
                Code = baseDeviceInfo.Code
            });
        }

        public void UpdateLatsLog(string serial, DateTime? dateTime)
        {
            if (DeviceConnection.TryGetValue(serial, out DeviceConnectionInfo devConn))
                devConn.LastLogItem = dateTime;
        }
    }
}