using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateGroup
{
    public class UpdateGroupCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public List<int> DeviceIds { get; set; }
    }
}