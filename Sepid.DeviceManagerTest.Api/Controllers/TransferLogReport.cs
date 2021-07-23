using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sepid.DeviceManagerTest.Api.Controllers;
using Sepid.DeviceManagerTest.Application.Common.Excel;
using Sepid.DeviceManagerTest.Application.Common.Pagination;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Application.Core.Report.Dto;
using Sepid.DeviceManagerTest.Application.Core.Report.Queries;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManager.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransferLogReportController : BaseController
    {
        private readonly IMediator _mediator;

        public TransferLogReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(PagedList<TransferLogDto>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTransferLogPagedListQuery filterTransferLogPagedListQuery, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(filterTransferLogPagedListQuery, cancellationToken));


        [ProducesResponseType(typeof(ReportDto), 200)]
        [HttpGet("ExcelReport")]
        public async Task<IActionResult> GetExcelList([FromQuery] GetTransferLogListQuery getTransferLogList,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(getTransferLogList, cancellationToken);

            var url = ExportTransferLog.TransferLog(result);

            return Ok(new ReportDto { Url = url });
        }
    }
}