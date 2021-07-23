using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Core.Logs.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Logs.Queries
{
    public class GetLogCountQuery : IRequest<Result<LogCountDto>>
    {
        public int DeviceId { get; set; }
    }

    public class GetLogCountQueryHandler : IRequestHandler<GetLogCountQuery, Result<LogCountDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;


        public GetLogCountQueryHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result<LogCountDto>> Handle(GetLogCountQuery request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result<LogCountDto>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));

            if (device.IsConnected == false)
                return Result<LogCountDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost, cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            return Result<LogCountDto>.SuccessFul(new LogCountDto { TotalLog = device.CurrentLogCount.HasValue ? (int)device.CurrentLogCount : 0 });
        }
    }
}