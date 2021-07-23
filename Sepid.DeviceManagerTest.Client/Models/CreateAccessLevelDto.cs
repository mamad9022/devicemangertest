using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class CreateAccessLevelDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public List<DoorScheduleDto> DoorSchedules { get; set; }
    }

    public class DoorScheduleDto
    {
        public uint DoorId { get; set; }

        public uint ScheduleId { get; set; }
    }

    public class CreateAccessLevelCommand
    {
        public string DeviceSerial { get; set; }

        public List<CreateAccessLevelDto> CreateAccessLevel { get; set; }
    }

    public class RemoveAccessLevelCommand
    {
        public string DeviceSerial { get; set; }

        public List<uint> AccessLevelId { get; set; }
    }
}