using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.Logs.Queries;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    public class LogController : BaseController
    {
        private readonly IMediator _mediator;

        public LogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetLog([FromQuery] GetLogQuery getLogQuery,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(getLogQuery, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpGet("Count/{deviceId}")]
        public async Task<IActionResult> GetLogCount(int deviceId)
        {
            var result = await _mediator.Send(new GetLogCountQuery { DeviceId = deviceId });

            return result.ApiResult;
        }
    }
}