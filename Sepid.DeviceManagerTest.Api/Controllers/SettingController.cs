using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.License.Command.InquiryLicense;
using Sepid.DeviceManagerTest.Application.Core.Settings.Command.SyncDeviceDatabase;
using Sepid.DeviceManagerTest.Application.Core.Settings.Command.Update;
using Sepid.DeviceManagerTest.Application.Core.Settings.Dto;
using Sepid.DeviceManagerTest.Application.Core.Settings.Queries;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SettingController : BaseController
    {
        private readonly IMediator _mediator;

        public SettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(SettingDto), 200)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetSettingQuery(), cancellation);

            return result.ApiResult;
        }

        [ProducesResponseType( 204)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateSettingCommand updateSettingCommand,
            CancellationToken cancellation)
        {
            var result = await _mediator.Send(updateSettingCommand, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();

        }


        [HttpPut("SyncDeviceDatabase")]
        public async Task<IActionResult> UpdateDeviceSDatabase(CancellationToken cancellation)
        {
            await _mediator.Send(new SyncDeviceDatabaseCommand(), cancellation);

            return NoContent();
        }


        [HttpGet("GetVersion")]
        public async Task<IActionResult> GetVersion(CancellationToken cancellationToken)
        {
            return Ok(new { version = await _mediator.Send(new GetSystemVersionQuery(), cancellationToken) });
        }


        [HttpPost("InquiryLicense")]
        public async Task<IActionResult> InquiryLicense(InquiryLicenseCommand inquiryLicenseCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(inquiryLicenseCommand, cancellationToken);

            return result.ApiResult;
        }
    }
}
