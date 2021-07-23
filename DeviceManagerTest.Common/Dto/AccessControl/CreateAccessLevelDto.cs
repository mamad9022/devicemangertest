using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Common.Dto.AccessControl
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
}