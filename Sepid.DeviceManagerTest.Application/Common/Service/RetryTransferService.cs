using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Core.User.Command.DeleteUser;
using Sepid.DeviceManagerTest.Application.Core.User.Command.EnrollUser;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Options;

namespace Sepid.DeviceManagerTest.Application.Common.Service
{
    public class RetryTransferService : ITransferService
    {
        private readonly IDeviceManagerContext _context;
        private readonly IOptionsMonitor<Setting> _settings;
        private readonly IMapper _mapper;

        public RetryTransferService(IDeviceManagerContext context, IOptionsMonitor<Setting> settings, IMapper mapper)
        {
            _context = context;
            _settings = settings;
            _mapper = mapper;
        }

        public async Task Retry()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(CancellationToken.None);

            var transferEventLogs = await _context.TransferLogs.Where(x =>
                x.Retry < setting.RetryFailedTransferNumber && x.IsSuccess == false &&
                x.CreateDate >= DateTime.Now.AddDays(-2)).ToListAsync();


            foreach (var failedLogs in transferEventLogs)
            {
                #region enrollment retry

                if (failedLogs.TransferLogType == TransferLogType.Enrollment)
                {
                    var data = JsonSerializer.Deserialize<EnrollUserCommand>(failedLogs.Data);

                    var device = await _context.Devices
                        .Include(x => x.DeviceModel)
                        .FirstOrDefaultAsync(x => x.Serial == data.DeviceSerial);

                    //if device remove in middle of process shout down that request
                    if (device is null)
                    {
                        failedLogs.Retry = _settings.CurrentValue.RetryFailedTransferNumber;
                        _context.Save();
                        continue;
                    }

                    #region Detect Strategy Interface

                    ServiceStrategyContext context = new ServiceStrategyContext();

                    context.DetectServices(device.DeviceModel.SdkType);

                    #endregion Detect Strategy Interface

                    var result = context.EnrollUser(new BaseDeviceInfoDto
                    {
                        Code = device.DeviceModel.Code,
                        Ip = device.Ip,
                        Port = device.Port,
                        Serial = device.Serial
                    }, _mapper.Map<UserDto>(data));

                    failedLogs.IsSuccess = result.Success;
                    failedLogs.Retry += 1;
                    failedLogs.ErrorMessage = result.Success == false ? result.Message : "";

                    await _context.SaveAsync(CancellationToken.None);
                }

                #endregion

                #region Delete User Retry

                else
                {
                    var data = JsonSerializer.Deserialize<DeleteUserCommand>(failedLogs.Data);

                    var device = await _context.Devices
                        .Include(x => x.DeviceModel)
                        .FirstOrDefaultAsync(x => x.Id == data.DeviceId);

                    //if device remove in middle of process shout down that request
                    if (device is null)
                    {
                        failedLogs.Retry = _settings.CurrentValue.RetryFailedTransferNumber;
                        await _context.SaveAsync(CancellationToken.None);
                        continue;
                    }

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
                    }, data.PersonCode);

                    failedLogs.IsSuccess = result.Success;
                    failedLogs.Retry += 1;
                    failedLogs.ErrorMessage = result.Success == false ? result.Message : "";

                    _context.Save();

                }


                #endregion


            }
        }
    }
}