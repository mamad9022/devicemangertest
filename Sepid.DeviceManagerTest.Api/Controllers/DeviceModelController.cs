using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Create;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Delete;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Update;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Queries;
using Sepid.DeviceManagerTest.Common.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeviceModelController : BaseController
    {
        private readonly IMediator _mediator;

        public DeviceModelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Device Model With Pagination
        /// </summary>
        /// <param name="getDeviceModelPagedListQuery"></param>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="400">if paging option out of range  </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(PagedList<DeviceModelDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetDeviceModelPagedListQuery getDeviceModelPagedListQuery)
            => Ok(await _mediator.Send(getDeviceModelPagedListQuery));


        /// <summary>
        /// Get Device Model With NoPagination
        /// </summary>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(List<DeviceModelDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("List")]
        public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetDeviceModelListQuery()));

        /// <summary>
        /// Get Device Model info
        /// </summary>
        /// <response code="200">if List Back Successfully </response>
        /// <response code="404">if device model  not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(DeviceModelDto), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet("{id}", Name = "getDeviceModelInfo")]
        public async Task<IActionResult> GetDeviceModelInfo(int id)
        {
            var result = await _mediator.Send(new GetDeviceModelQuery { Id = id });

            return result.ApiResult;
        }

        /// <summary>
        /// Delete Device Model
        /// </summary>
        /// <response code="204">if Device model Delete Successfully </response>
        /// <response code="404">if device model  not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteDeviceModelCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        /// <summary>
        /// create Device Model
        /// </summary>
        /// <response code="201">if Device model Create Successfully </response>
        /// <response code="400">if Validation Failed </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(typeof(DeviceModelDto), 201)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDeviceModelCommand createDeviceModel)
        {
            var result = await _mediator.Send(createDeviceModel);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("getDeviceModelInfo", new { id = result.Data.Id }), result.Data);
        }

        /// <summary>
        /// update Device Model
        /// </summary>
        /// <response code="204">if Device update Successfully </response>
        /// <response code="404">if device model  not found </response>
        /// <response code="500">If an unexpected error happen</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDeviceModelCommand updateDeviceCommand)
        {
            var result = await _mediator.Send(updateDeviceCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}