using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.Stats.Queries;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Application.Core.Stats.Dto;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : BaseController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(StatDto), 200)]
        [HttpGet]
        public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetStatsQuery(), cancellationToken));


    }
}
