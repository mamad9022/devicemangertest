using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Application.Core.Groups.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.CreateGroup
{
    public class CreateGroupCommand : IRequest<Result<GroupDto>>
    {

        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public List<int> DeviceIds { get; set; }
    }
}