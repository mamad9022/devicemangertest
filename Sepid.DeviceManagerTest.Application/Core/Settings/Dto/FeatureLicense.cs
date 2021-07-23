using System;

namespace Sepid.DeviceManagerTest.Application.Core.Settings.Dto
{
    public class FeatureLicense
    {
        public int PersonNumber { get; set; }
        public int DeviceNumber { get; set; }
        public string SystemId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}