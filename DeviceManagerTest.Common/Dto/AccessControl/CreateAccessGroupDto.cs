using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Common.Dto.AccessControl
{
    public class CreateAccessGroupDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        public List<uint> AccessGroupIds { get; set; }
    }
}