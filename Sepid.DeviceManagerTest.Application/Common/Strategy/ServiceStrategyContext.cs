using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.DeviceServices;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.AccessControl;
using Sepid.DeviceManagerTest.Common.Dto.AuthConfig;
using Sepid.DeviceManagerTest.Common.Dto.DeviceLog;
using Sepid.DeviceManagerTest.Common.Dto.Door;
using Sepid.DeviceManagerTest.Common.Dto.Schedules;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ZK;
using ZK.Common;

namespace Sepid.DeviceManagerTest.Application.Common.Strategy
{
    public class ServiceStrategyContext
    {
        private IDeviceOperationServices _services;
        private readonly IBusPublish _busPublish;
        private readonly IDeviceManagerContext _context;
        private readonly IWebHostEnvironment _environment;
        //private readonly ITemplateService _templateService;
        //private IMatchOnServerService _matchOnServerService;


        public ServiceStrategyContext(IBusPublish busPublish, IDeviceManagerContext context,
            IWebHostEnvironment environment)
        {
            _busPublish = busPublish;
            _context = context;
            _environment = environment;
            //_templateService = templateService;
        }

        public ServiceStrategyContext()
        {
        }

        public ServiceStrategyContext(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //public ServiceStrategyContext(ITemplateService templateService)
        //{
        //    _templateService = templateService;
        //}

        public IDeviceOperationServices DetectServices(SdkType sdkType)
        {
            switch (sdkType)
            {
                case SdkType.ZkTechno:
                    IZksdk _zksdk = new Zksdk();
                    return _services = new ZKServices(_zksdk);

                //case SdkType.BioStarV2:
                //    return _services = new BioStarV2Services(_environment);

                default:
                    Log.Error("service Method  Argument out of range Exception");
                    throw new ArgumentOutOfRangeException();
            }
        }

        //public IMatchOnServerService DetectMatching(SdkType sdkType)
        //{
        //switch (sdkType)
        //{
        //    case SdkType.:
        //        return _matchOnServerService = new BioStarV2MatchOnServerService(_templateService);

        //    default:
        //        Log.Error("service Method  Argument out of range Exception");
        //        throw new ArgumentOutOfRangeException();
        //}
        //   }

        #region Config

        public Result<DateTime> GetTime(BaseDeviceInfoDto baseDeviceInfo) => _services.GetTime(baseDeviceInfo);

        public Result<bool> SetTime(BaseDeviceInfoDto baseDeviceInfo, DateTime date) =>
            _services.SetTime(baseDeviceInfo, date);

        public Result ClearLog(BaseDeviceInfoDto baseDeviceInfoDto) => _services.ClearLog(baseDeviceInfoDto);

        public Result RebootDevice(BaseDeviceInfoDto baseDeviceInfo) => _services.RebootDevice(baseDeviceInfo);

        public Result FactoryReset(BaseDeviceInfoDto baseDeviceInfo) => _services.FactoryReset(baseDeviceInfo);

        public Result LockDevice(BaseDeviceInfoDto baseDeviceInfo) => _services.LockDevice(baseDeviceInfo);

        public Result UnlockDevice(BaseDeviceInfoDto baseDeviceInfo) => _services.UnlockDevice(baseDeviceInfo);

        public Result ActiveImageLog(BaseDeviceInfoDto baseDeviceInfo) => _services.ActiveImageLog(baseDeviceInfo);

        public Result DeactivateImageLog(BaseDeviceInfoDto baseDeviceInfo) => _services.DeactivateImage(baseDeviceInfo);

        public Result Disconnect(BaseDeviceInfoDto baseDeviceInfo) => _services.Disconnect(baseDeviceInfo);

        #endregion Config

        #region Log

        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task<Result<List<LogInfo>>> GetLogs(FilteredDeviceLogDto filteredDeviceLog)
        {
            _services ??= DetectServices(filteredDeviceLog.SdkType);



            var logs = await _services.GetLogs(filteredDeviceLog);

            var device = _context.Devices.FirstOrDefault(x => x.Serial == filteredDeviceLog.Serial);

            //set connection after each retrieve log from device and set last date time retrieve log
            if (logs.Success)
            {
                if (device != null)
                {
                    device.IsConnected = true;
                    var date = logs.Data.OrderByDescending(x => x.EventTime).FirstOrDefault();

                    if (date != null)
                    {
                        device.LastLogRetrieve = date.EventTime;
                        device.LastRetrievedLogId = date.Id;
                    }

                    await _context.SaveAsync(CancellationToken.None);
                }

                var filterLogs = logs.Data.Where(x => x.AttendanceType != AttendanceType.Undefined).ToList();

                if (filterLogs.Any())
                    foreach (var log in filterLogs)
                        _busPublish.Send("DeviceLog", JsonSerializer.Serialize(log));

                foreach (var log in logs.Data.ToList())
                    _busPublish.Send("eventLogs", JsonSerializer.Serialize(log));
            }

            else
            {
                if (device != null)
                {
                    device.IsConnected = false;
                    _context.Save();
                }
            }

            return logs;
        }

        public Result<int> GetLogCount(BaseDeviceInfoDto baseDeviceInfo) => _services.LogCount(baseDeviceInfo);

        #endregion

        #region Auth

        public Result<AuthConfigDto> GetAuthConfig(BaseDeviceInfoDto baseDeviceInfoDto) =>
            _services.GetAuthConfig(baseDeviceInfoDto);

        public Result SetAuthConfig(BaseDeviceInfoDto baseDeviceInfoDto, SetAuthConfigDto setAuthConfigDto) =>
            _services.SetAuthConfig(baseDeviceInfoDto, setAuthConfigDto);

        #endregion

        #region Scan

        public Result<ScanResultDto> ScanFinger(BaseDeviceInfoDto baseDevice, int quality) => _services.ScanFinger(baseDevice, quality);

        public Result<ScanResultDto> ScanCard(BaseDeviceInfoDto baseDeviceInfo) => _services.ScanCard(baseDeviceInfo);

        public Result<ScanResultDto> ScanFace(BaseDeviceInfoDto baseDeviceInfo) => _services.ScanFace(baseDeviceInfo);

        #endregion Scan

        #region User

        public Result DeleteAllUser(BaseDeviceInfoDto baseDeviceInfoDto) => _services.DeleteAllUser(baseDeviceInfoDto);

        public Result DeleteUser(BaseDeviceInfoDto baseDeviceInfoDto, string personCode) =>
            _services.DeleteUser(baseDeviceInfoDto, personCode);

        public Result EnrollUser(BaseDeviceInfoDto baseDeviceInfo, UserDto userDto) =>
            _services.EnrollUser(baseDeviceInfo, userDto);

        public Result<List<UserDto>> GetAllUser(BaseDeviceInfoDto baseDeviceInfo) =>
            _services.GetAllUser(baseDeviceInfo);

        #endregion User

        #region Network

        public Result<NetworkInfoDto> GetNetworkInfo(BaseDeviceInfoDto baseDeviceInfo) =>
            _services.GetNetWorkConfig(baseDeviceInfo);

        public Result<NetworkInfoDto> SetNetWorkConfig(BaseDeviceInfoDto baseDeviceInfo, NetworkInfoDto networkInfo) =>
            _services.SetNetworkConfig(baseDeviceInfo, networkInfo);

        #endregion Network

        #region Door

        public Result<List<DoorStatusDto>> GetDoorStatus(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds) =>
            _services.GetDoorStatus(baseDeviceInfo, doorIds);

        public Result<List<DoorDto>> GetDoorConfig(BaseDeviceInfoDto baseDeviceInfo) =>
            _services.GetDoorList(baseDeviceInfo);

        public Result SetDoorConfig(BaseDeviceInfoDto baseDeviceInfo, CreateDoorDto createDoor) =>
            _services.SetDoor(baseDeviceInfo, createDoor);

        public Result RemoveDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds) =>
            _services.RemoveDoor(baseDeviceInfo, doorIds);

        public Result ReleaseDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag) =>
            _services.ReleaseDoor(baseDeviceInfo, doorIds, doorFlag);

        public Result SetAlarm(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorAlarmFlagEnum doorAlarmFlag) =>
            _services.SetAlarm(baseDeviceInfo, doorIds, doorAlarmFlag);

        public Result LockDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag) =>
            _services.LockDoor(baseDeviceInfo, doorIds, doorFlag);

        public Result UnlockDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag) =>
            _services.UnlockDoor(baseDeviceInfo, doorIds, doorFlag);

        #endregion Door

        #region Schedule

        public Result SetHolidayGroupDto(BaseDeviceInfoDto baseDeviceInfo, List<HolidayGroupDto> holidayGroup) =>
            _services.SetHolidayGroup(baseDeviceInfo, holidayGroup);

        public Result RemoveHolidayGroup(BaseDeviceInfoDto baseDeviceInfo, List<uint> holidayGroupIds) =>
            _services.RemoveHolidayGroup(baseDeviceInfo, holidayGroupIds);

        public Result<List<HolidayGroupDto>> GetHolidayGroup(BaseDeviceInfoDto baseDeviceInfo) =>
            _services.GetHolidayGroups(baseDeviceInfo);

        public Result RemoveSchedule(BaseDeviceInfoDto baseDeviceInfo, List<uint> scheduleIds) =>
            _services.RemoveSchedule(baseDeviceInfo, scheduleIds);

        public Result<List<ScheduleDto>> GetScheduleList(BaseDeviceInfoDto baseDeviceInfo) =>
            _services.GetScheduleList(baseDeviceInfo);

        public Result SetSchedule(BaseDeviceInfoDto baseDeviceInfo, List<CreateScheduleDto> createScheduleDto) =>
            _services.SetSchedule(baseDeviceInfo, createScheduleDto);

        #endregion Schedule

        #region AccessGroup

        public Result
            CreateAccessGroup(BaseDeviceInfoDto baseDeviceInfo, List<CreateAccessGroupDto> createAccessGroup) =>
            _services.SetAccessGroup(baseDeviceInfo, createAccessGroup);

        public Result<List<AccessGroupDto>> GetAccessGroup(BaseDeviceInfoDto baseDeviceInfo) =>
            _services.GetAccessGroupList(baseDeviceInfo);

        public Result RemoveAccessGroup(BaseDeviceInfoDto baseDeviceInfo, List<uint> accessGroupId) =>
            _services.RemoveAccessGroup(baseDeviceInfo, accessGroupId);

        public Result SetAccessLevel(BaseDeviceInfoDto baseDeviceInfo, List<CreateAccessLevelDto> createAccessLevel) =>
            _services.SetAccessLevel(baseDeviceInfo, createAccessLevel);

        public Result<List<AccessLevelDto>> GetAccessLevel(BaseDeviceInfoDto baseDeviceInfo) =>
            _services.GetAccessLevel(baseDeviceInfo);

        public Result RemoveAccessLevel(BaseDeviceInfoDto baseDeviceInfo, List<uint> accessLevelId) =>
            _services.RemoveAccessLevel(baseDeviceInfo, accessLevelId);

        #endregion AccessGroup
        public Result ConnectionStatus(BaseDeviceInfoDto baseDeviceInfo) => _services.ConnectionStatus(baseDeviceInfo);

        #region MatchOnServer

        //public Result ServerMatching(BaseDeviceInfoDto baseDeviceInfo) => _matchOnServerService.ServerMatching(baseDeviceInfo);

        //public Result DetachServerMatching(BaseDeviceInfoDto baseDeviceInfo) =>
        //    _matchOnServerService.DetachServerMatching(baseDeviceInfo);

        #endregion
    }
}