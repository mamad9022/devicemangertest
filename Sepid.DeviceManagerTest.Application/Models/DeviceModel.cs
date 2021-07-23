using Sepid.DeviceManagerTest.Common.Enum;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Models
{
    public class DeviceModel
    {
        public DeviceModel()
        {
            Devices = new HashSet<Device>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int TotalLog { get; set; }
        public DeviceModelTypes Code { get; set; }
        public SdkType SdkType { get; set; }
        public bool IsFingerSupport { get; set; }
        public bool IsFaceSupport { get; set; }
        public bool IsCardSupport { get; set; }
        public bool IsPasswordSupport { get; set; }

        public ICollection<Device> Devices { get; }
    }
}