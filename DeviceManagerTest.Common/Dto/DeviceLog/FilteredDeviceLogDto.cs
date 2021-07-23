using System;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Common.Dto.DeviceLog
{
    public class FilteredDeviceLogDto : BaseDeviceInfoDto
    {
        public DateTime? FromDate { get; set; }

        public DateTime ToDate { get; set; } = DateTime.Now;

        public long? LastLogId { get; set; }

        public SdkType SdkType { get; set; }
    }
}