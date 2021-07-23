using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.Schedules;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.Schedules.Queries
{
    public class GetScheduleQuery : IRequest<Result<List<ScheduleDto>>>
    {
        public string DeviceSerial { get; set; }
    }

    public class GetScheduleQueryHandler : IRequestHandler<GetScheduleQuery, Result<List<ScheduleDto>>>
    {
        private readonly IDeviceManagerContext _context;

        public GetScheduleQueryHandler(IDeviceManagerContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ScheduleDto>>> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Serial == request.DeviceSerial, cancellationToken);

            if (device is null)
                return Result<List<ScheduleDto>>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.DeviceNotFound)));

            if (device.IsConnected==false)
                return Result<List<ScheduleDto>>.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.ConnectionLost)));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var result = context.GetScheduleList(new BaseDeviceInfoDto
            {
                Ip = device.Ip,
                Serial = device.Serial,
                Port = device.Port,
                Code = device.DeviceModel.Code
            });

            if (result.Success == false)
                return Result<List<ScheduleDto>>.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            return Result<List<ScheduleDto>>.SuccessFul(result.Data);
        }
    }
}