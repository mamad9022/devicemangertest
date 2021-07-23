using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.CreateGroup;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.DeleteGroup;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateDeviceGroup;
using Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateGroup;
using Sepid.DeviceManagerTest.Application.Core.Groups.Queries;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    public class GroupController : BaseController
    {
        private readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions, CancellationToken cancellation)
            => Ok(await _mediator.Send(new GetGroupPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Query = pagingOptions.Query
            }, cancellation));


        [HttpGet("List")]
        public async Task<IActionResult> GetList(CancellationToken cancellation)
            => Ok(await _mediator.Send(new GetGroupListQuery(), cancellation));


        [HttpGet("{id}", Name = "GetGroupInfo")]
        public async Task<IActionResult> GetInfo(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetGroupQuery { Id = id }, cancellation);

            return result.ApiResult;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupCommand createGroupCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createGroupCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return CreatedAtAction(nameof(GetInfo), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateGroupCommand updateGroupCommand, CancellationToken cancellation)
        {
            var result = await _mediator.Send(updateGroupCommand, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteGroupCommand { Id = id }, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpGet("Devices/{id}")]
        public async Task<IActionResult> GetDevices([FromRoute] int id,
            [FromQuery] PagingOptions pagingOptions,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetDeviceInGroupPagedListQuery
            {
                GroupId = id,
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Query = pagingOptions.Query
            }, cancellationToken);

            return result.ApiResult;
        }


        [HttpPut("UpdateDeviceGroup")]
        public async Task<IActionResult> UpdateDeviceGroup(UpdateDeviceGroupCommand updateDeviceGroupCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateDeviceGroupCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}