using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Dto
{
    public class GroupDto
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual List<GroupDto> Children { get; set; }
    }


    public class GroupViewModel
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string Name { get; set; }

        public List<int> DeviceIds { get; set; }

    }
}