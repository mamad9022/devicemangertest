using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.Schedules.Command.CreateHolidayGroup;
using Sepid.DeviceManagerTest.Application.Core.Schedules.Command.CreateSchedule;
using Sepid.DeviceManagerTest.Application.Core.Schedules.Command.RemoveHolidayGroup;
using Sepid.DeviceManagerTest.Application.Core.Schedules.Command.RemoveSchedule;
using Sepid.DeviceManagerTest.Application.Core.Schedules.Queries;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    public class SchedulesController : BaseController
    {
        private readonly IMediator _mediator;

        public SchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("HolidayGroup/{deviceSerial}")]
        public async Task<IActionResult> GetHolidayGroup(string deviceSerial)
        {
            var result = await _mediator.Send(new GetHolidayGroupQuery { DeviceSerial = deviceSerial });

            return result.ApiResult;
        }

        [HttpPost("addHolidayGroup")]
        public async Task<IActionResult> CreateHolidayGroup(CreateHolidayGroupCommand createHolidayGroupCommand)
        {
            var result = await _mediator.Send(createHolidayGroupCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPut("RemoveHolidayGroup")]
        public async Task<IActionResult> RemoveHolidayGroup(RemoveHolidayGroupCommand removeHolidayGroup)
        {
            var result = await _mediator.Send(removeHolidayGroup);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpGet("GetSchedule/{deviceSerial}")]
        public async Task<IActionResult> GetSchedule(string deviceSerial)
        {
            var result = await _mediator.Send(new GetScheduleQuery { DeviceSerial = deviceSerial });

            return result.ApiResult;
        }

        [HttpPut("RemoveSchedule")]
        public async Task<IActionResult> RemoveSchedule(RemoveScheduleCommand removeSchedule)
        {
            var result = await _mediator.Send(removeSchedule);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPost("SetSchedule")]
        public async Task<IActionResult> SetSchedule(CreateScheduleCommand createScheduleCommand)
        {
            var result = await _mediator.Send(createScheduleCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}