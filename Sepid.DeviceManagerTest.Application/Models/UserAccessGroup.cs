using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Models
{
    public class UserAccessGroup
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool IsDeleted { get; set; }

        public List<long> GroupIds { get; set; }
    }
}