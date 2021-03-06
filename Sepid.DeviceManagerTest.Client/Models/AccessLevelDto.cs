using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class AccessLevelDto
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public List<uint> DoorId { get; set; }

        public List<uint> ScheduleId { get; set; }
    }
}