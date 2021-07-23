using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.AccessControl;
using Sepid.DeviceManagerTest.Common.Dto.AuthConfig;
using Sepid.DeviceManagerTest.Common.Dto.DeviceLog;
using Sepid.DeviceManagerTest.Common.Dto.Door;
using Sepid.DeviceManagerTest.Common.Dto.Schedules;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Common.DeviceServices
{
    public interface IDeviceOperationServices
    {
      

        #region Config Device

        Result<DateTime> GetTime(BaseDeviceInfoDto baseDeviceInfo);

        Result<bool> SetTime(BaseDeviceInfoDto baseDeviceInfo, DateTime date);

        Result ClearLog(BaseDeviceInfoDto baseDeviceInfo);

        Result RebootDevice(BaseDeviceInfoDto baseDeviceInfo);

        Result FactoryReset(BaseDeviceInfoDto baseDeviceInfo);

        Result LockDevice(BaseDeviceInfoDto baseDeviceInfo);

        Result UnlockDevice(BaseDeviceInfoDto baseDeviceInfo);

        DeviceInfo Search(BaseDeviceInfoDto baseDeviceInfo);

        List<DeviceInfo> BroadCastSearch();

        Result ActiveImageLog(BaseDeviceInfoDto baseDeviceInfo);

        Result DeactivateImage(BaseDeviceInfoDto baseDeviceInfo);

        Result Disconnect(BaseDeviceInfoDto baseDeviceInfo);

        #endregion Config Device

        #region log

        Task<Result<List<LogInfo>>> GetLogs(FilteredDeviceLogDto filteredDeviceLog);

        Result<int> LogCount(BaseDeviceInfoDto baseDeviceInfo);


        #endregion

        #region Scan

        Result<ScanResultDto> ScanFinger(BaseDeviceInfoDto baseDeviceInfo, int quality);

        Result<ScanResultDto> ScanCard(BaseDeviceInfoDto baseDeviceInfo);

        Result<ScanResultDto> ScanFace(BaseDeviceInfoDto baseDeviceInfo);

        #endregion Scan

        #region User

        Result<List<UserDto>> GetAllUser(BaseDeviceInfoDto baseDeviceInfo);

        Result EnrollUser(BaseDeviceInfoDto baseDeviceInfo, UserDto user);

        Result DeleteAllUser(BaseDeviceInfoDto baseDeviceInfo);

        Result DeleteUser(BaseDeviceInfoDto baseDeviceInfo, string personCode);

        Result<UserDto> GetUserInfo(BaseDeviceInfoDto baseDeviceInfo, string personCode);

        #endregion User

        #region NetworkConfig

        Result<NetworkInfoDto> GetNetWorkConfig(BaseDeviceInfoDto baseDeviceInfo);

        Result<NetworkInfoDto> SetNetworkConfig(BaseDeviceInfoDto baseDeviceInfo, NetworkInfoDto networkInfo);

        #endregion NetworkConfig

        #region Door

        Result<List<DoorDto>> GetDoorList(BaseDeviceInfoDto baseDeviceInfo);

        Result SetDoor(BaseDeviceInfoDto baseDeviceInfo, CreateDoorDto createDoor);

        Result<List<DoorStatusDto>> GetDoorStatus(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds);

        Result RemoveDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds);

        Result ReleaseDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag);

        Result SetAlarm(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorAlarmFlagEnum doorAlarmFlag);

        Result LockDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag);

        Result UnlockDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag);

        #endregion Door

        #region Access Control

        Result<List<AccessGroupDto>> GetAccessGroupList(BaseDeviceInfoDto baseDeviceInfo);

        Result RemoveAccessGroup(BaseDeviceInfoDto baseDeviceInfo, List<uint> accessGroupId);

        Result SetAccessGroup(BaseDeviceInfoDto baseDeviceInfo, List<CreateAccessGroupDto> createAccessGroup);

        Result<List<AccessLevelDto>> GetAccessLevel(BaseDeviceInfoDto baseDeviceInfo);

        Result RemoveAccessLevel(BaseDeviceInfoDto baseDeviceInfo, List<uint> accessLevelIds);

        Result SetAccessLevel(BaseDeviceInfoDto baseDeviceInfo, List<CreateAccessLevelDto> createAccessLevels);

        Result<List<ScheduleDto>> GetScheduleList(BaseDeviceInfoDto baseDeviceInfo);

        Result RemoveSchedule(BaseDeviceInfoDto baseDeviceInfo, List<uint> scheduleIds);

        Result SetSchedule(BaseDeviceInfoDto baseDeviceInfo, List<CreateScheduleDto> createSchedules);

        public Result<List<HolidayGroupDto>> GetHolidayGroups(BaseDeviceInfoDto baseDeviceInfo);

        public Result RemoveHolidayGroup(BaseDeviceInfoDto baseDeviceInfo, List<uint> holidayGroupId);

        public Result SetHolidayGroup(BaseDeviceInfoDto baseDeviceInfo, List<HolidayGroupDto> holidayGroup);

        #endregion Access Control

        #region AuthConfig

        Result<AuthConfigDto> GetAuthConfig(BaseDeviceInfoDto baseDeviceInfoDto);

        Result SetAuthConfig(BaseDeviceInfoDto baseDeviceInfoDto, SetAuthConfigDto setAuthConfigDto);

        #endregion

        void ConnectDeviceToServer(int port);

        Result ConnectionStatus(BaseDeviceInfoDto baseDeviceInfo);
    }
}