using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Application.Core.Files.Command;
using Sepid.DeviceManagerTest.Common.Helper;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Api.Controllers
{
    public class FilesController : BaseController
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// uploading files
        /// </summary>
        /// <remarks>
        ///
        /// Types can be one of this: Avatar, Image, Video, Music, Document, Other
        /// count validate with [X-MultiSelect] Header can between 1 to 20
        ///
        /// </remarks>
        /// <param name="createFileCommand"></param>
        /// <returns></returns>
        /// <response code="200">Get Files list Uploaded</response>
        /// <response code="400">If validation failure.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesErrorResponseType(typeof(ApiMessage))]
        [HttpPost("Upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateFileCommand createFileCommand)
        {
            var result = await _mediator.Send(createFileCommand);

            return result.ApiResult;
        }
    }
}