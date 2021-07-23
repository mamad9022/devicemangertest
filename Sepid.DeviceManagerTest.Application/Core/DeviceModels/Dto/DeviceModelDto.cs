using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto
{
    public class DeviceModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DeviceModelTypes Code { get; set; }
        public SdkType SdkType { get; set; }
        public bool IsFingerSupport { get; set; }
        public bool IsFaceSupport { get; set; }
        public bool IsCardSupport { get; set; }
        public bool IsPasswordSupport { get; set; }
        public int TotalLog { get; set; }
    }
}