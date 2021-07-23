using System;
using System.Threading;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Core.Report.Dto;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sepid.DeviceManagerTest.Application.Common.Service
{
    public class BatchOperationService : IBatchOperationService
    {
        private readonly IDeviceManagerContext _context;
        private readonly IBusPublish _busPublish;

        public BatchOperationService(IDeviceManagerContext context, IBusPublish busPublish)
        {
            _context = context;
            _busPublish = busPublish;
        }

        public async Task SetTime(long groupId, DateTime dateTime,CancellationToken cancellationToken)
        {
            var groups = await _context.Groups
                .Include(x => x.DeviceInGroups)
                .ThenInclude(x => x.Device)
                .ThenInclude(x => x.DeviceModel)
                .FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
         
            foreach (var deviceInGroup in groups.DeviceInGroups)
            {
                if (deviceInGroup.Device.IsConnected)
                {
                    ServiceStrategyContext context = new();

                    context.DetectServices(deviceInGroup.Device.DeviceModel.SdkType);

                    var result = context.SetTime(new BaseDeviceInfoDto
                    {
                        Serial = deviceInGroup.Device.Serial,
                        Ip = deviceInGroup.Device.Ip,
                        Port = deviceInGroup.Device.Port,
                        Code = deviceInGroup.Device.DeviceModel.Code,

                    }, dateTime);

                    if (result.Success)
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"تنظیم ساعت دستگاه {deviceInGroup.Device.Name} ",
                            Description = $"ساعت دستگاه با موفقیت به {dateTime.ToShortPersianDateTimeString()} تغییر پیدا کرد",
                            LogType = LogType.Success
                        }));
                    }
                    else
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"تنظیم ساعت دستگاه خطا در {deviceInGroup.Device.Name} ",
                            Description = result.Message,
                            LogType = LogType.Error
                        }));
                    }
                }
                else
                {
                    _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                    {
                        Title = $"تنظیم ساعت دستگاه خطا در {deviceInGroup.Device.Name} ",
                        Description = $"ارتباط با دستگاه برقرار نمی باشد",
                        LogType = LogType.Error
                    }));
                }
            }
        }

        public async Task LockDevice(long groupId, CancellationToken cancellationToken)
        {
            var groups = await _context.Groups
                .Include(x => x.DeviceInGroups)
                .ThenInclude(x => x.Device)
                .ThenInclude(x => x.DeviceModel)
                .FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);

            foreach (var deviceInGroup in groups.DeviceInGroups)
            {
                if (deviceInGroup.Device.IsConnected && deviceInGroup.Device.IsLock == false && deviceInGroup.Device.DeviceModel.SdkType == SdkType.ZkTechno)
                {
                    ServiceStrategyContext context = new();

                    context.DetectServices(deviceInGroup.Device.DeviceModel.SdkType);

                    var result = context.LockDevice(new BaseDeviceInfoDto
                    {
                        Serial = deviceInGroup.Device.Serial,
                        Ip = deviceInGroup.Device.Ip,
                        Port = deviceInGroup.Device.Port,
                        Code = deviceInGroup.Device.DeviceModel.Code,

                    });

                    if (result.Success)
                    {

                        deviceInGroup.Device.IsLock = true;
                        await _context.SaveAsync(cancellationToken);

                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"قفل شدن دستگاه {deviceInGroup.Device.Name} ",
                            Description = $"قفل شدن دستگاه {deviceInGroup.Device.Name} در تاریخ {DateTime.Now.ToLongPersianDateTimeString()}",
                            LogType = LogType.Success
                        }));

                    }
                    else
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"قفل شدن دستگاه {deviceInGroup.Device.Name} ",
                            Description = result.Message,
                            LogType = LogType.Error
                        }));
                    }

                }
                else
                {
                    if (deviceInGroup.Device.DeviceModel.SdkType == SdkType.ZkTechno)
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"قفل شدن دستگاه {deviceInGroup.Device.Name} ",
                            Description = $"عدم پشتیبانی دستگاه از فرمان قفل شدن دستگاه",
                            LogType = LogType.Error
                        }));
                    }
                    else
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"قفل شدن دستگاه {deviceInGroup.Device.Name} ",
                            Description = $"ارتباط با دستگاه برقرار نمی باشد",
                            LogType = LogType.Error
                        }));
                    }

                }
            }

        }

        public async Task UnlockDevice(long groupId, CancellationToken cancellationToken)
        {
            var groups = await _context.Groups
                .Include(x => x.DeviceInGroups)
                .ThenInclude(x => x.Device)
                .ThenInclude(x => x.DeviceModel)
                .FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
            foreach (var deviceInGroup in groups.DeviceInGroups)
            {
                if (deviceInGroup.Device.IsConnected && deviceInGroup.Device.IsLock && deviceInGroup.Device.DeviceModel.SdkType == SdkType.ZkTechno)
                {
                    ServiceStrategyContext context = new();

                    context.DetectServices(deviceInGroup.Device.DeviceModel.SdkType);

                    var result = context.UnlockDevice(new BaseDeviceInfoDto
                    {
                        Serial = deviceInGroup.Device.Serial,
                        Ip = deviceInGroup.Device.Ip,
                        Port = deviceInGroup.Device.Port,
                        Code = deviceInGroup.Device.DeviceModel.Code,

                    });

                    if (result.Success)
                    {

                        deviceInGroup.Device.IsLock = false;
                        await _context.SaveAsync(cancellationToken);

                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $" باز شدن قفل  دستگاه  {deviceInGroup.Device.Name} ",
                            Description = $" باز شدن قفل  دستگاه  {deviceInGroup.Device.Name} در تاریخ {DateTime.Now.ToLongPersianDateTimeString()}",
                            LogType = LogType.Success
                        }));

                    }
                    else
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"باز شدن قفل  دستگاه {deviceInGroup.Device.Name} ",
                            Description = result.Message,
                            LogType = LogType.Error
                        }));
                    }

                }
                else
                {
                    if (deviceInGroup.Device.DeviceModel.SdkType == SdkType.ZkTechno)
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"باز شدن قفل شدن دستگاه  {deviceInGroup.Device.Name} ",
                            Description = $"عدم پشتیبانی دستگاه از فرمان قفل شدن دستگاه",
                            LogType = LogType.Error
                        }));
                    }
                    else
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"باز شدن قفل شدن دستگاه {deviceInGroup.Device.Name} ",
                            Description = $"ارتباط با دستگاه برقرار نمی باشد",
                            LogType = LogType.Error
                        }));
                    }

                }
            }

        }

        public async Task ClearLog(long groupId, CancellationToken cancellationToken)
        {
            var groups = await _context.Groups
                .Include(x => x.DeviceInGroups)
                .ThenInclude(x => x.Device)
                .ThenInclude(x => x.DeviceModel)
                .FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
          
            
            foreach (var deviceInGroup in groups.DeviceInGroups)
            {
                if (deviceInGroup.Device.IsConnected)
                {
                    ServiceStrategyContext context = new();

                    context.DetectServices(deviceInGroup.Device.DeviceModel.SdkType);

                    var result = context.ClearLog(new BaseDeviceInfoDto
                    {
                        Serial = deviceInGroup.Device.Serial,
                        Ip = deviceInGroup.Device.Ip,
                        Port = deviceInGroup.Device.Port,
                        Code = deviceInGroup.Device.DeviceModel.Code,

                    });

                    if (result.Success)
                    {

                        deviceInGroup.Device.CurrentLogCount = 1;
                        await _context.SaveAsync(cancellationToken);

                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"حذف شدن لاگ دستگاه {deviceInGroup.Device.Name} ",
                            Description = $" حذف شدن لاگ دستگاه {deviceInGroup.Device.Name} در تاریخ {DateTime.Now.ToLongPersianDateTimeString()}",
                            LogType = LogType.Success
                        }));

                    }
                    else
                    {
                        _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                        {
                            Title = $"حذف شدن لاگ دستگاه {deviceInGroup.Device.Name}",
                            Description = result.Message,
                            LogType = LogType.Error
                        }));
                    }

                }
                else
                {

                    _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                    {
                        Title = $"حذف شدن لاگ دستگاه {deviceInGroup.Device.Name} ",
                        Description = $"ارتباط با دستگاه برقرار نمی باشد",
                        LogType = LogType.Error
                    }));
                }
            }

        }
    }
}