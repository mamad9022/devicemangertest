using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.BatchOperation.ClearLog;
using Sepid.DeviceManagerTest.Application.Core.BatchOperation.LockDevice;
using Sepid.DeviceManagerTest.Application.Core.BatchOperation.SetTime;
using Sepid.DeviceManagerTest.Application.Core.BatchOperation.UnlockDevice;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BatchOperationController : BaseController
    {
        private readonly IMediator _mediator;

        public BatchOperationController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("SetTime")]
        public async Task<IActionResult> SetTimeOperation(SetTimeOperationCommand setTimeCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(setTimeCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost("LockDevice")]
        public async Task<IActionResult> LockDevice(LockDeviceOperationCommand lockDeviceCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(lockDeviceCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost("UnlockDevice")]
        public async Task<IActionResult> UnlockDeviceOperation(UnlockDeviceOperationCommand unlockDeviceCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(unlockDeviceCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPost("ClearLog")]
        public async Task<IActionResult> ClearLogOperation(ClearLogOperationCommand clearLogCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(clearLogCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }

    }
}