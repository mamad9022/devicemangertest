using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Dto;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanCard
{
    public class ScanCardCommandHandler : IRequestHandler<ScanCardCommand, Result<TemplateDataDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public ScanCardCommandHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result<TemplateDataDto>> Handle(ScanCardCommand request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result<TemplateDataDto>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            if (device.DeviceModel.IsCardSupport == false)
                return Result<TemplateDataDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotSupportFinger,cancellationToken))));

            if (device.IsConnected==false)
                return Result<TemplateDataDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost,cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Sdk Device

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Sdk Device

            var result = context.ScanCard(new BaseDeviceInfoDto
            {
                Ip = device.Ip,
                Serial = device.Serial,
                Port = device.Port
            });

            if (result.Success == false)
                return Result<TemplateDataDto>.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            return Result<TemplateDataDto>.SuccessFul(new TemplateDataDto
            {
                TemplateData = result.Data.Template,
                Image = Convert.ToBase64String(result.Data.TemplateData) 
            });
        }
    }
}