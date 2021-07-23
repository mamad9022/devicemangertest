using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.AccessControl;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.AccessGroup.Queries
{
    public class GetAccessGroupQuery : IRequest<Result<List<AccessGroupDto>>>
    {
        public string DeviceSerial { get; set; }
    }

    public class GetAccessGroupQueryHandler : IRequestHandler<GetAccessGroupQuery, Result<List<AccessGroupDto>>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        public GetAccessGroupQueryHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result<List<AccessGroupDto>>> Handle(GetAccessGroupQuery request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Serial == request.DeviceSerial, cancellationToken);

            if (device is null)
                return Result<List<AccessGroupDto>>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            if (device.IsConnected==false)
                return Result<List<AccessGroupDto>>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost,cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var result = context.GetAccessGroup(new BaseDeviceInfoDto
            {
                Ip = device.Ip,
                Serial = device.Serial,
                Port = device.Port,
                Code = device.DeviceModel.Code
            });

            if (result.Success == false)
                return Result<List<AccessGroupDto>>.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            return Result<List<AccessGroupDto>>.SuccessFul(result.Data);
        }
    }
}