using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Models
{
    public class Group
    {
        public Group()
        {
            DeviceInGroups = new HashSet<DeviceInGroup>();
            Children = new HashSet<Group>();
        }

        public long Id { get; set; }

        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Group Parent { get; set; }
        public virtual ICollection<Group> Children { get; }
        public virtual ICollection<DeviceInGroup> DeviceInGroups { get; }

    }

    public class DeviceInGroup
    {
        public long GroupId { get; set; }

        public int DeviceId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Device Device { get; set; }
    }


}