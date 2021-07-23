using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class CreateAccessGroupDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        public List<uint> AccessGroupIds { get; set; }
    }

    public class CreateAccessGroupCommand
    {
        public string DeviceSerial { get; set; }

        public List<CreateAccessGroupDto> CreateAccessGroup { get; set; }
    }

    public class RemoveAccessGroupCommand
    {
        public string DeviceSerial { get; set; }
        public List<uint> AccessGroupId { get; set; }
    }
}