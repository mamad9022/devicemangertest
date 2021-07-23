using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.User.Command.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public DeleteUserCommandHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var result = context.DeleteUser(new BaseDeviceInfoDto
            {
                Code = device.DeviceModel.Code,
                Ip = device.Ip,
                Port = device.Port,
                Serial = device.Serial
            }, request.PersonCode);


            await _context.TransferLogs.AddAsync(new TransferLog
            {
                CreateDate = DateTime.Now,
                IsSuccess = result.Success,
                Data = JsonSerializer.Serialize(request),
                TransferLogType = TransferLogType.DeleteUser,
                ErrorMessage = result.Success == false ? result.Message : "",
                Description = $"{request.Name} - {request.PersonCode}",
                Retry = 1,
                DeviceId = device.Id,
                Reason = result.Reason
            }, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            if (result.Success == false)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            return Result.SuccessFul();
        }
    }
}