using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Dto;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanFinger
{
    public class ScanFingerCommandHandler : IRequestHandler<ScanFingerCommand, Result<FingerDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        private const string KEY = "92A826412D074EB7B6D4712C4C9237E1";
        private const string IV = "1891823B13D344B6";

        public ScanFingerCommandHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result<FingerDto>> Handle(ScanFingerCommand request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            var setting = await _context.Settings.FirstOrDefaultAsync(cancellationToken);

            if (device is null)
                return Result<FingerDto>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            if (device.DeviceModel.IsFingerSupport == false)
                return Result<FingerDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotSupportFinger,cancellationToken))));

            if (device.IsConnected == false)
                return Result<FingerDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost,cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var result = context.ScanFinger(new BaseDeviceInfoDto
            {
                Ip = device.Ip,
                Port = device.Port,
                Serial = device.Serial
            }, setting.FingerPrintQuality);

            if (result.Success == false)
                return Result<FingerDto>.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            return Result<FingerDto>.SuccessFul(new FingerDto
            {
                TemplateData = EncryptProvider.AESEncrypt(Convert.ToBase64String(result.Data.TemplateData),KEY,IV),
                Quality = result.Data.Quality,
                Image = Convert.ToBase64String(result.Data.TemplateImage)
            });
        }
    }
}