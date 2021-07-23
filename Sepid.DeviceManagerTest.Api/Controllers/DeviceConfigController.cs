using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.BroadCastSearchCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.ClearLogCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.FactoryResetCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.GetTimeCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.LockDeviceCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.RebootDeviceCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SearchDeviceCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetNetworkCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetTimeCommand;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Dto;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Queries;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.ActiveImageCommand;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeviceConfigController : BaseController
    {
        private readonly IMediator _mediator;

        public DeviceConfigController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Time Device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="400">if device Can not connect   </response>
        /// <response code="404">if Device Not Found  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(DeviceDateDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("getTime/{deviceId}")]
        public async Task<IActionResult> GetTime(int deviceId)
        {
            var result = await _mediator.Send(new GetTimeCommand { DeviceId = deviceId });

            return result.ApiResult;
        }

        /// <summary>
        /// Set Device Time Device
        /// </summary>
        /// <param name="setTimeCommand"></param>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="400">if device Can not connect   </response>
        /// <response code="404">if Device Not Found  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost("setTime")]
        public async Task<IActionResult> SetTime(SetTimeCommand setTimeCommand)
        {
            var result = await _mediator.Send(setTimeCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// To Clear all Log from Device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="204">if log successfully Delete from device </response>
        /// <response code="404">if Device Not Found  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("ClearLog/{deviceId}")]
        public async Task<IActionResult> ClearLog(int deviceId)
        {
            var result = await _mediator.Send(new ClearLogCommand { DeviceId = deviceId });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Broad Cast In Network To Find All device Available
        /// </summary>
        /// <response code="200">if Device Found Successfully </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<DeviceSearchDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("BroadCastSearch")]
        public async Task<IActionResult> BroadCastDevice()
        {
            var result = await _mediator.Send(new BroadCastSearchCommand());

            return Ok(result);
        }

        /// <summary>
        /// Search Device In Network By Ip
        /// </summary>
        /// <param name="searchDeviceCommand"></param>
        /// <response code="200">if Device Found Successfully </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(DeviceSearchDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("SearchDevice")]
        public async Task<IActionResult> SearchDevice([FromQuery] SearchDeviceCommand searchDeviceCommand)
        {
            var result = await _mediator.Send(searchDeviceCommand);

            return Ok(result);
        }

        /// <summary>
        /// in Case you Want to reset your device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="204">if device reboot successfully</response>
        /// <response code="400">if can not connect to device or operation complete </response>
        /// <response code="404">if device not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("ResetDevice/{deviceId}")]
        public async Task<IActionResult> RebootDevice(int deviceId)
        {
            var result = await _mediator.Send(new RebootDeviceCommand { Id = deviceId });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// in Case you Want to reset to default setting
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="204">if device reset factory successfully</response>
        /// <response code="400">if can not connect to device or operation complete </response>
        /// <response code="404">if device not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("factoryReset/{deviceId}")]
        public async Task<IActionResult> FactoryReset(int deviceId)
        {
            var result = await _mediator.Send(new FactoryResetCommand { Id = deviceId });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// in Case you Want to Lock Device an no one can authorize from device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="204">if device lock successfully</response>
        /// <response code="400">if can not connect to device or operation complete </response>
        /// <response code="404">if device not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("UpdateLockDevice/{deviceId}")]
        public async Task<IActionResult> LockDevice(int deviceId)
        {
            var result = await _mediator.Send(new LockDeviceCommand { Id = deviceId });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }



        /// <summary>
        /// get network config of device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="200">if device reboot successfully</response>
        /// <response code="400">if can not connect to device or operation complete </response>
        /// <response code="404">if device not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(NetworkInfoDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("GetNetworkConfig/{deviceId}")]
        public async Task<IActionResult> GetNetworkConfig(int deviceId)
        {
            var result = await _mediator.Send(new GetNetworkConfigDeviceQuery { DeviceId = deviceId });

            return result.ApiResult;
        }

        /// <summary>
        /// set network Command
        /// </summary>
        /// <param name="setNetWorkCommand"></param>
        /// <response code="200">if device reboot successfully</response>
        /// <response code="400">if can not connect to device or operation complete </response>
        /// <response code="404">if device not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(NetworkInfoDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost("SetNetWorkConfig")]
        public async Task<IActionResult> SetNetWorkConfig(SetNetWorkCommand setNetWorkCommand)
        {
            var result = await _mediator.Send(setNetWorkCommand);

            return result.ApiResult;
        }


        [HttpGet("ActivationImageLog/{deviceId}")]
        public async Task<IActionResult> ActiveImageLog(int deviceId)
        {
            var result = await _mediator.Send(new ActivateImageLogCommand { DeviceId = deviceId });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();

        }
    }
}