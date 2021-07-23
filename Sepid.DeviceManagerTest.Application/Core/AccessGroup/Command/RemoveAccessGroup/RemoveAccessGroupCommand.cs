using MediatR;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.AccessGroup.Command.RemoveAccessGroup
{
    public class RemoveAccessGroupCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }
        public List<uint> AccessGroupId { get; set; }
    }
}