using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.UpdateConnectionCommand
{
    public class UpdateConnectionCommandHandler : IRequestHandler<UpdateConnectionCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public UpdateConnectionCommandHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(Command.UpdateConnectionCommand.UpdateConnectionCommand request, CancellationToken cancellationToken)
        {


            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));



            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            Result result;
            if (device.IsConnected)
            {
                result = context.Disconnect(new BaseDeviceInfoDto
                {
                    Serial = device.Serial,
                    Ip = device.Ip,
                    Port = device.Port
                });
            }
            else
            {
                //result = context.ConnectionStatus(new BaseDeviceInfoDto
                //{
                //    Serial = device.Serial,
                //    Ip = device.Ip,
                //    Port = device.Port
                //});
            }


            //if (result.Success == false)
            //    return Result.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            device.IsActive = !device.IsActive;
            device.IsConnected = !device.IsConnected;

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();

        }
    }
}