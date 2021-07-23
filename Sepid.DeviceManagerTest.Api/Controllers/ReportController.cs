using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.Report.Queries;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("DeviceStats")]
        public async Task<IActionResult> GetDeviceStatsReport(CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetDeviceStatsQuery(), cancellationToken));
    }
}
