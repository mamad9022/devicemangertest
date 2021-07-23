using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Common.Dto.AccessControl
{
    public class AccessGroupDto
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public List<uint> AccessLevels { get; set; }

        public List<uint> FloorLevel { get; set; }

        public byte NumAccessLevels { get; set; }
        public byte NumFloorLevels { get; set; }
    }
}