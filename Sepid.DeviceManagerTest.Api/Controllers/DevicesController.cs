using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Common.Excel;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Create;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Delete;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Diagnose;
using Sepid.DeviceManagerTest.Application.Core.Devices.Command.Update;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Application.Core.Devices.Queries;
using Sepid.DeviceManagerTest.Application.Core.User.Command.DeleteAllUser;
using Sepid.DeviceManagerTest.Application.Core.User.Command.DeleteUser;
using Sepid.DeviceManagerTest.Application.Core.User.Command.EnrollUser;
using Sepid.DeviceManagerTest.Application.Core.User.Queries;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.UpdateConnectionCommand;
using Sepid.DeviceManagerTest.Application.Core.User.Command.SendUserToDatabase;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DevicesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILocalization _localization;

        public DevicesController(IMediator mediator, ILocalization localization)
        {
            _mediator = mediator;
            _localization = localization;
        }

        /// <summary>
        /// Get List Of Device with Pagination
        /// </summary>

        /// <param name="getDevicePagedList"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="400">if paging option out of range  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(PagedList<DeviceListDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> GetDevicePagedList([FromQuery] GetDevicePagedListQuery getDevicePagedList, CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(getDevicePagedList, cancellationToken));

        /// <summary>
        /// Get List Of Device with no Pagination
        /// </summary>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<DeviceListDto>), 200)]
        [HttpGet("list")]
        public async Task<IActionResult> GetList() => Ok(await _mediator.Send(new GetDeviceListQuery()));

        [ProducesResponseType(typeof(ReportDto), 200)]
        [HttpGet("ExcelReport")]
        public async Task<IActionResult> GetExcelReport([FromQuery] GetDeviceListQuery getDeviceListQuery)
        {
            var devices = await _mediator.Send(getDeviceListQuery);

            var url = ExportDevice.DeviceList(devices);

            return Ok(new ReportDto { Url = url });
        }

        /// <summary>
        /// Get DeviceInfo
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="404">if device not found </response>
        /// <response code="400">if paging option out of range  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(DeviceDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("{id}", Name = "GetDeviceInfo")]
        public async Task<IActionResult> GetDeviceInfo(int id)
        {
            var result = await _mediator.Send(new GetDeviceQuery { Id = id });

            return result.ApiResult;
        }

        /// <summary>
        /// Delete Device Info
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">if List Back Successfully </response>
        /// <response code="404">if device not found </response>
        /// <response code="400">if paging option out of range  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDeviceCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Create Device Info
        /// </summary>
        /// <param name="createDeviceCommand"></param>
        /// <response code="201">if User Create Successfully </response>
        /// <response code="404">if device not found </response>
        /// <response code="400">if Validation Failed </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(DeviceDto), 201)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDeviceCommand createDeviceCommand)
        {
            var result = await _mediator.Send(createDeviceCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetDeviceInfo", new { id = result.Data.Id }), result.Data);
        }

        /// <summary>
        /// update  Device Info
        /// </summary>
        /// <param name="updateDeviceCommand"></param>
        /// <response code="204">if User Update Successfully </response>
        /// <response code="404">if device not found </response>
        /// <response code="400">if Validation Failed </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDeviceCommand updateDeviceCommand)
        {
            var result = await _mediator.Send(updateDeviceCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Delete All User exist In Device
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">if Delete User from Successfully </response>
        /// <response code="404">if device not found </response>
        /// <response code="400">if Validation Failed Happen</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpDelete("DeleteAllUser/{id}")]
        public async Task<IActionResult> DeleteAllUserFromDevice(int id)
        {
            var result = await _mediator.Send(new DeleteAllUserCommand { DeviceId = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Delete User From Device with person code
        /// </summary>
        /// <param name="deleteUserCommand"></param>
        /// <response code="204">if Delete User  Successfully </response>
        /// <response code="400">if Validation Failed Happen</response>
        /// <response code="404">if device not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserCommand deleteUserCommand)
        {
            var result = await _mediator.Send(deleteUserCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Enroll User To Device
        /// </summary>
        /// <param name="enrollUserCommand"></param>
        /// <response code="204">if Enroll User  Successfully </response>
        /// <response code="404">if device not found </response>
        /// <response code="400">if Validation Failed Happen</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost("EnrollUser")]
        public async Task<IActionResult> EnrollUser(EnrollUserCommand enrollUserCommand)
        {
            var result = await _mediator.Send(enrollUserCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// Get  List All User From Device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <response code="204">if Enroll User  Successfully </response>
        /// <response code="404">if device not found </response>
        /// <response code="400">if Validation Failed Happen</response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<UserDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("GetAllUser/{deviceId}")]
        public async Task<IActionResult> GetAllUser(int deviceId)
        {
            var result = await _mediator.Send(new GetUserListQuery { DeviceId = deviceId });

            if (result.Success == false)
                return result.ApiResult;

            return Ok(result.Data);
        }

        [ProducesResponseType(typeof(ApiMessage), 200)]
        [HttpGet("{id}/Diagnostic")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new DiagnoseCommand { DeviceId = id });

            if (result.Success == false)
                return result.ApiResult;

            return Ok(new ApiMessage(_localization.GetMessage(ResponseMessage.DeviceSuccessfullyConnected)));
        }


        [HttpPost("SendUserToDatabase")]
        public async Task<IActionResult> SendUser(SendUserToDatabaseCommand sendUserToDatabaseCommand,
            CancellationToken cancellation)
        {
            var result = await _mediator.Send(sendUserToDatabaseCommand, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPut("UpdateConnection/{Id}")]
        public async Task<IActionResult> UpdateDeviceConnection(long id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateConnectionCommand { Id = id }, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }




    }
}