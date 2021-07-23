using System.Data.SqlClient;
using System.Linq;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sepid.DeviceManagerTest.Application.Core.Report.Dto;
using Sepid.DeviceManagerTest.Application.Core.User.Command.SendUserToDatabase;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface IDeviceServices
    {
        Task LogCount();

        Task AutoClearLog();

        Task SyncDeviceDatabase();

        Task AutoClearHangFireJob();
    }

    public class DeviceServices : IDeviceServices
    {
        private readonly IDeviceManagerContext _context;
        private readonly IBusPublish _busPublish;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public DeviceServices(IDeviceManagerContext context, IBusPublish busPublish, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _busPublish = busPublish;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task LogCount()
        {
            var activeDevices = await _context.Devices.Include(x => x.DeviceModel)
                .Where(x => x.IsConnected).ToListAsync();



            foreach (var device in activeDevices)
            {
                ServiceStrategyContext context = new ServiceStrategyContext();
                context.DetectServices(device.DeviceModel.SdkType);

                var result = context.GetLogCount(new BaseDeviceInfoDto
                {
                    Serial = device.Serial,
                    Ip = device.Ip,
                    Port = device.Port,
                    Code = device.DeviceModel.Code,
                    IsDeviceToServer = device.IsDeviceToServer,
                    ServerIp = device.ServerIp,
                    ServerPort = device.ServerPort,
                    MaxLogCount = device.DeviceModel.TotalLog
                });

                if (result.Success)
                {
                    device.CurrentLogCount = result.Data;

                }

            }

            _context.Save();
        }

        public async Task AutoClearLog()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();

            if (setting.EnableClearLog)
            {
                var activeDevices = await _context.Devices
                    .Include(x => x.DeviceModel)
                    .Where(x => x.IsConnected && x.CurrentLogCount != null).ToListAsync();


                foreach (var device in activeDevices)
                {
                    double devicePercentage = (((double)device.CurrentLogCount!.Value * 100) / (double)device.DeviceModel.TotalLog);

                    ServiceStrategyContext context = new ServiceStrategyContext();
                    context.DetectServices(device.DeviceModel.SdkType);

                    if (devicePercentage >= setting.AutoClearLogPercentage)
                    {
                        var result = context.ClearLog(new BaseDeviceInfoDto
                        {
                            Serial = device.Serial,
                            Code = device.DeviceModel.Code,
                            Ip = device.Ip,
                            Port = device.Port
                        });

                        if (result.Success)
                        {
                            device.CurrentLogCount = 1;
                            _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                            {
                                Title = $"تخلیه اتوماتیک دستگاه {device.Name}",
                                Description = $"دستگاه با ظرفیت {devicePercentage}% با شماره سریال {device.Serial} تخلیه شد",
                                LogType = LogType.Info
                            }));
                        }
                    }

                }

                _context.Save();
            }

        }

        public async Task SyncDeviceDatabase()
        {
            var activeDevices = await _context.Devices
                .Include(x => x.DeviceModel)
                .Where(x => x.IsConnected).ToListAsync();

            foreach (var device in activeDevices)
            {
                ServiceStrategyContext context = new ServiceStrategyContext();
                context.DetectServices(device.DeviceModel.SdkType);

                var userData = context.GetAllUser(new BaseDeviceInfoDto
                {
                    Serial = device.Serial,
                    Ip = device.Ip,
                    Port = device.Port,
                    ServerIp = device.ServerIp,
                    Code = device.DeviceModel.Code,
                    IsDeviceToServer = device.IsDeviceToServer
                });

                if (userData.Success == false)
                {
                    _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                    {
                        Title = $"عدم ارسال کاربر از دستگاه {device.Name} ",
                        Description = userData.Message,
                        LogType = LogType.Error
                    }));
                    continue;
                }

                foreach (var users in userData.Data)
                {
                    var person = _mapper.Map<SendUserToDatabaseCommand>(users);

                    person.DeviceId = device.Id;

                    _busPublish.Send("UpdatePerson", JsonSerializer.Serialize(person));
                }

                _busPublish.Send("Notification", JsonSerializer.Serialize(new NotificationDto
                {
                    Title = $"ارسال اطلاعات از دستگاه {device.Name} به طور کامل انجام شد ",
                }));

            }
        }

        public Task AutoClearHangFireJob()
        {
            SqlConnection conn = new SqlConnection
            {
                ConnectionString = _configuration.GetConnectionString("DeviceManagerContext")
            };
            conn.Open();
            SqlCommand command = new SqlCommand("delete from HangFire.Job", conn);
            command.ExecuteNonQuery();

            conn.Close();
            conn.Dispose();

            return Task.CompletedTask;

        }
    }
}
