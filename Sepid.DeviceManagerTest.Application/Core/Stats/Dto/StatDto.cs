using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Stats.Dto
{
    public class StatDto
    {
        public int TotalDevice { get; set; }

        public int ActiveDevice { get; set; }

        public int DeactivateDevice { get; set; }

        public List<OverFlowLogDevice> overFlowLogDevices{get;set;}
    }

    public class OverFlowLogDevice
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Serial { get; set; }

        public double Percentage { get; set; }

        public long? CurrentLog { get; set; }

        public int TotalLog { get; set; }
    }
}
