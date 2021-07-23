using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Api.Controllers;
using Sepid.DeviceManagerTest.Application.Core.Auth.Command;
using Sepid.DeviceManagerTest.Application.Core.Auth.Queries;
using Sepid.DeviceManagerTest.Common.Dto.AuthConfig;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManager.Api.Controllers
{
   
    public class AuthConfigController : BaseController
    {
        private readonly IMediator _mediator;

        public AuthConfigController(IMediator mediator)
        {
            _mediator = mediator;
        }
       
        
        [ProducesResponseType(typeof(AuthConfigDto),200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [HttpGet("{deviceId}")]
        public async Task<IActionResult> Get(int deviceId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAuthConfigQuery {DeviceId = deviceId}, cancellationToken);

            return result.ApiResult;
        }


        [HttpPost]
        public async Task<IActionResult> Post(SetAuthConfigCommand setAuthConfigCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(setAuthConfigCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;


            return NoContent();
        }
    }
}