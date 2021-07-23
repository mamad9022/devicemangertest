using System;

namespace Sepid.DeviceManagerTest.Application.Core.License.Dto
{
    public class FeatureLicenseDto
    {
        public int PersonNumber { get; set; }
        public int DeviceNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public string MacAddress { get; set; }
    }
}