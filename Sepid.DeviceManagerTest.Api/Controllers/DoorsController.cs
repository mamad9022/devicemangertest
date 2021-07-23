using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Command.RemoveDoor;
using Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Command.SetDoor;
using Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Command.StatusCommand;
using Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Queries;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    public class DoorsController : BaseController
    {
        private readonly IMediator _mediator;

        public DoorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetDoor(int deviceId)
        {
            var result = await _mediator.Send(new GetDoorConfigQuery { DeviceId = deviceId });

            return result.ApiResult;
        }

        [HttpPost("GetDoorStatus")]
        public async Task<IActionResult> DoorStatus(DoorStatusCommand doorStatusCommand)
        {
            var result = await _mediator.Send(doorStatusCommand);

            return result.ApiResult;
        }

        [HttpPost]
        public async Task<IActionResult> SetDoor(SetDoorCommand setDoorCommand)
        {
            var result = await _mediator.Send(setDoorCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPut("Remove")]
        public async Task<IActionResult> RemoveDoor(RemoveDoorCommand removeDoorCommand)
        {
            var result = await _mediator.Send(removeDoorCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}