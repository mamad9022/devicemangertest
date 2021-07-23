using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;
using Serilog;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Command.Diagnose
{
    public class DiagnoseCommandHandler : IRequestHandler<DiagnoseCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public DiagnoseCommandHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DiagnoseCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));

            #endregion Validation

            bool pingAble = false;
            Ping ping = null;

            #region Ping Section

            try
            {
                ping = new Ping();
                var reply = ping.Send(device.Ip);
                if (reply != null) pingAble = reply.Status == IPStatus.Success;

                if (pingAble == false)
                    return Result.Failed(new OkObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceHasNotPing, cancellationToken))));
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex.Message);
                return Result.Failed(new BadRequestObjectResult(await _localization.GetMessage(ResponseMessage.UnHandleError, cancellationToken)));
            }
            finally
            {
                ping?.Dispose();
            }

            #endregion Ping Section

            #region Detect Strategy Interface

            ServiceStrategyContext context = new();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            //var status = context.ConnectionStatus(new BaseDeviceInfoDto
            //{
            //    Ip = device.Ip,
            //    Serial = device.Serial,
            //    Port = device.Port,
            //    Code = device.DeviceModel.Code,
            //    ServerPort = device.ServerPort,
            //    ServerIp = device.ServerIp,
            //    IsDeviceToServer = device.IsMatchOnServer
            //});

            //if (status.Success == false)
            //    return Result.Failed(new BadRequestObjectResult(new ApiMessage(status.Message)));

            device.IsConnected = true;
            device.IsActive = true;
            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}