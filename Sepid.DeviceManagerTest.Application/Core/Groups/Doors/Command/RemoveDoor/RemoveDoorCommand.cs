using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Command.RemoveDoor
{
    public class RemoveDoorCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
        public List<int> DoorIds { get; set; }
    }
}