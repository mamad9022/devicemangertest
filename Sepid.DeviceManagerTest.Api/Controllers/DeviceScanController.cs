using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanCard;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanFace;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanFinger;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeviceScanController : BaseController
    {
        private readonly IMediator _mediator;

        public DeviceScanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Scan Finger
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="200">if Scan Finger Successfully </response>
        /// <response code="400">if device Can not connect   </response>
        /// <response code="404">if Device Not Found  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(FingerDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("ScanFinger/{deviceId}")]
        public async Task<IActionResult> ScanFinger(int deviceId)
        {
            var result = await _mediator.Send(new ScanFingerCommand { DeviceId = deviceId });

            return result.ApiResult;
        }

        /// <summary>
        /// Scan card
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="200">if Scan card Successfully </response>
        /// <response code="400">if device Can not connect   </response>
        /// <response code="404">if Device Not Found  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(TemplateDataDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("ScanCard/{deviceId}")]
        public async Task<IActionResult> ScanCard(int deviceId)
        {
            var result = await _mediator.Send(new ScanCardCommand { DeviceId = deviceId });

            return result.ApiResult;
        }

        /// <summary>
        /// Scan Face
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="200">if Scan face Successfully </response>
        /// <response code="400">if device Can not connect   </response>
        /// <response code="404">if Device Not Found  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(TemplateDataDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("ScanFace/{deviceId}")]
        public async Task<IActionResult> ScanFace(int deviceId)
        {
            var result = await _mediator.Send(new ScanFaceCommand { DeviceId = deviceId });

            return result.ApiResult;
        }
    }
}