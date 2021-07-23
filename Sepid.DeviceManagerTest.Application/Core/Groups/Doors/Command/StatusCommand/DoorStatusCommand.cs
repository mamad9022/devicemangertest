using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Dto.Door;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Command.StatusCommand
{
    public class DoorStatusCommand : IRequest<Result<List<DoorStatusDto>>>
    {
        public int DeviceId { get; set; }

        public List<int> DoorIds { get; set; }
    }
}