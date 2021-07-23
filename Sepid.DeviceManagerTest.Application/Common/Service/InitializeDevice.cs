using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.DeviceServices;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.DeviceLog;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sepid.DeviceManager.Application.Common.Service
{
    public class InitializeDeviceToServer : IInitializeDeviceToServer
    {
        private readonly IDeviceManagerContext _context;
        private readonly IEnumerable<IDeviceOperationServices> _services;
        //private readonly ITemplateService _templateService;
        private readonly IBusPublish _busPublish;
        private readonly IWebHostEnvironment _environment;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InitializeDeviceToServer(IDeviceManagerContext context, IEnumerable<IDeviceOperationServices> services, IBusPublish busPublish, IWebHostEnvironment environment, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _services = services;
            //_templateService = templateService;
            _busPublish = busPublish;
            _environment = environment;
            _serviceScopeFactory = serviceScopeFactory;
        }


        public void Init()
        {
            var devicesPort = _context.Devices.Where(x => x.IsMatchOnServer)
                .GroupBy(x => x.ServerPort)
                .Select(x => x.Key).ToList();

            foreach (var service in _services)
            {
                devicesPort.ForEach(port =>
                {
                    if (port != null)
                        service.ConnectDeviceToServer(port.Value);
                });
            }

            var deviceV2 = _context.Devices.Where(x => x.IsMatchOnServer == false && x.DeviceModel.SdkType == SdkType.ZkTechno).ToList();

            foreach (var device in deviceV2)
            {
                ServiceStrategyContext context = new();

                context.DetectServices(SdkType.ZkTechno);

             //   context.ConnectionStatus(new BaseDeviceInfoDto { Ip = device.Ip, Serial = device.Serial });
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(3600)]
        public async Task EvacuationNonVitalDevice()
        {

            //var setting = await _context.Settings.FirstOrDefaultAsync();

            //if (string.IsNullOrWhiteSpace(setting.License))
            //    return;

            //var license = EncryptProvider.AESDecrypt(setting.License, KEY, IV);

            //var featureLicense = JsonSerializer.Deserialize<FeatureLicense>(license);


            //if (featureLicense.ExpireDate < DateTime.Now)
            //{
            //    return;
            //}


            int page = 0;
            int limit = 20;

            var allTask = new List<Task>();
            while (true)
            {
                var devices = await _context.Devices.Where(x => x.IsVital == false && x.IsActive)
                    .Include(x => x.DeviceModel).Skip(page * limit).Take(limit).ToListAsync();

                if (devices.Count > 0)
                {
                    var t = Task.Factory.StartNew(async () => await EvacuationLog(devices), TaskCreationOptions.LongRunning);

                    allTask.Add(t);
                }
                else
                {
                    break;
                }

                page++;
            }

            Task.WhenAll(allTask).Wait();

            //Log.Information("All Task Completed");

        }

        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(3600)]
        public async Task EvacuationVitalDevice()
        {

            int page = 0;
            int limit = 3;
            var allTask = new List<Task>();
            while (true)
            {
                var devices = await _context.Devices.OrderByDescending(x => x.Id).Where(x => x.IsVital && x.IsActive)
                    .Include(x => x.DeviceModel).Skip(page * limit).Take(limit).ToListAsync();

                if (devices.Count > 0)
                {
                    var t = Task.Factory.StartNew(async () => await EvacuationLog(devices), TaskCreationOptions.LongRunning);

                    allTask.Add(t);

                }
                else
                {
                    break;
                }

                page++;
            }

            Task.WhenAll(allTask).Wait();
        }

        public void InitialMatchOnServer()
        {
            var devices = _context.Devices
                .Include(x => x.DeviceModel)
                .Where(x => x.IsMatchOnServer
                            && x.DeviceModel.SdkType == SdkType.ZkTechno).ToList();

            foreach (var device in devices)
            {
                ServiceStrategyContext context = new();

                context.DetectServices(device.DeviceModel.SdkType);

                //context.ServerMatching(new BaseDeviceInfoDto
                //{
                //    Code = device.DeviceModel.Code,
                //    Ip = device.Ip,
                //    Serial = device.Serial,
                //    Port = device.Port,
                //    IsDeviceToServer = device.IsDeviceToServer,
                //    ServerIp = device.ServerIp,
                //    ServerPort = device.ServerPort
                //});

            }


        }

        public async Task EvacuationLog(List<Device> devices)
        {

            foreach (var device in devices)
            {

                Log.Information($"device with serial {device.Serial} and ip {device.Ip}");
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetService<IDeviceManagerContext>();
                ServiceStrategyContext context = new(_busPublish, dbContext, _environment);

                context.DetectServices(device.DeviceModel.SdkType);

                await context.GetLogs(new FilteredDeviceLogDto
                {
                    Serial = device.Serial,
                    ServerIp = device.ServerIp,
                    Port = device.Port,
                    Ip = device.Ip,
                    Code = device.DeviceModel.Code,
                    SdkType = device.DeviceModel.SdkType,
                    IsDeviceToServer = device.IsDeviceToServer,
                    LastLogId = device.LastRetrievedLogId,
                    FromDate = device.LastLogRetrieve ?? device.SyncLogStartDate,

                });





            }
        }
    }
}