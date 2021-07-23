using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto.DeviceLog;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Logs.Queries
{
    public class GetLogQuery : IRequest<Result>
    {
        public int DeviceId { get; set; }

        public DateTime? FromDate { get; set; }
        
    }

    public class GetLogQueryHandler : IRequestHandler<GetLogQuery, Result>
    {
        private readonly IDeviceManagerContext _context;

        public GetLogQueryHandler(IDeviceManagerContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetLogQuery request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.DeviceNotFound)));

            if (device.IsConnected==false)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.ConnectionLost)));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface
            
            BackgroundJob.Enqueue(() => context.GetLogs(new FilteredDeviceLogDto
            {
                Serial = device.Serial,
                Ip = device.Ip,
                Code = device.DeviceModel.Code,
                FromDate = request.FromDate,
                Port = device.Port,
                SdkType = device.DeviceModel.SdkType
            }));

         

            return Result.SuccessFul();
        }
    }
}