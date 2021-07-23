using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.UnlockDeviceCommand
{
    public class UnlockDeviceCommandHandler : IRequestHandler<UnlockDeviceCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public UnlockDeviceCommandHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(UnlockDeviceCommand request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            if (device.IsConnected==false)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost,cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var result = context.UnlockDevice(new BaseDeviceInfoDto
            {
                Ip = device.Ip,
                Port = device.Port,
                Serial = device.Serial
            });

            if (result.Success == false)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            device.IsLock = false;
            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}