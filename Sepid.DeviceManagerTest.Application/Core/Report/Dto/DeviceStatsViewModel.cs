using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Report.Dto
{
    public class DeviceStatsViewModel
    {
        public int TotalDevice { get; set; }

        public List<DeviceStatsDto> ConnectedDevices { get; set; }

        public List<DeviceStatsDto> DeviceBrands { get; set; }
    }
}