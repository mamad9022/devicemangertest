using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Api.Controllers;
using Sepid.DeviceManagerTest.Application.Core.AccessGroup.Command.CreateAccessGroup;
using Sepid.DeviceManagerTest.Application.Core.AccessGroup.Command.CreateAccessLevel;
using Sepid.DeviceManagerTest.Application.Core.AccessGroup.Command.RemoveAccessGroup;
using Sepid.DeviceManagerTest.Application.Core.AccessGroup.Command.RemoveAccessLevel;
using Sepid.DeviceManagerTest.Application.Core.AccessGroup.Queries;
using System.Threading.Tasks;

namespace Sepid.DeviceManager.Api.Controllers
{
    public class AccessController : BaseController
    {
        private readonly IMediator _mediator;

        public AccessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SetAccessGroup")]
        public async Task<IActionResult> CreateAccessLevel(CreateAccessGroupCommand createAccessGroup)
        {
            var result = await _mediator.Send(createAccessGroup);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpGet("AccessGroup/{deviceSerial}")]
        public async Task<IActionResult> GetAccessGroup(string deviceSerial)
        {
            var result = await _mediator.Send(new GetAccessGroupQuery { DeviceSerial = deviceSerial });

            return result.ApiResult;
        }

        [HttpPut("RemoveAccessGroup")]
        public async Task<IActionResult> RemoveAccessGroup(RemoveAccessGroupCommand removeAccessGroupCommand)
        {
            var result = await _mediator.Send(removeAccessGroupCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPost("setAccessLevel")]
        public async Task<IActionResult> SetAccessLevel(CreateAccessLevelCommand createAccessLevel)
        {
            var result = await _mediator.Send(createAccessLevel);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpGet("AccessLevel/{deviceSerial}")]
        public async Task<IActionResult> GetAccessLevel(string deviceSerial)
        {
            var result = await _mediator.Send(new GetAccessLevelQuery { DeviceSerial = deviceSerial });

            return result.ApiResult;
        }

        [HttpPut("removeAccessLevel")]
        public async Task<IActionResult> RemoveAccessLevel(RemoveAccessLevelCommand removeAccessLevelCommand)
        {
            var result = await _mediator.Send(removeAccessLevelCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}