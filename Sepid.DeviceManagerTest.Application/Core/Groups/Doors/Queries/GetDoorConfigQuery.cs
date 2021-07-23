using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.Door;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Doors.Queries
{
    public class GetDoorConfigQuery : IRequest<Result<List<DoorDto>>>
    {
        public int DeviceId { get; set; }
    }

    public class GetDoorConfigQueryHandler : IRequestHandler<GetDoorConfigQuery, Result<List<DoorDto>>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public GetDoorConfigQueryHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result<List<DoorDto>>> Handle(GetDoorConfigQuery request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result<List<DoorDto>>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            if (device.IsConnected==false)
                return Result<List<DoorDto>>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost,cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var result = context.GetDoorConfig(new BaseDeviceInfoDto
            {
                Serial = device.Serial,
                Ip = device.Ip,
                Port = device.Port
            });

            if (result.Success == false)
                return Result<List<DoorDto>>
                    .Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            return Result<List<DoorDto>>.SuccessFul(result.Data);
        }
    }
}