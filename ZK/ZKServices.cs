using Sepid.DeviceManagerTest.Common.DeviceErrorMessage;
using Sepid.DeviceManagerTest.Common.DeviceServices;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.AccessControl;
using Sepid.DeviceManagerTest.Common.Dto.AuthConfig;
using Sepid.DeviceManagerTest.Common.Dto.DeviceLog;
using Sepid.DeviceManagerTest.Common.Dto.Door;
using Sepid.DeviceManagerTest.Common.Dto.Schedules;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZK.Business.ReadUserInfo;
using ZK.Common;

namespace ZK
{
    public class ZKServices : IDeviceOperationServices
    {
        private readonly Func<string, int, Result<bool>> _connectionStatus;
        private readonly IZksdk _zksdk;
        private const int Zk = 1;

        public ZKServices(IZksdk zksdk)
        {
            _connectionStatus = ConnectToDevice;
            _zksdk = zksdk;
        }

        public Result ActiveImageLog(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public List<DeviceInfo> BroadCastSearch()
        {
            throw new NotImplementedException();
        }

        public Result ClearLog(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device

            Log.Information($"try to clear log from device {baseDeviceInfo.Serial} ");

            _zksdk.ClearLog(baseDeviceInfo.MachineNumber);

            return Result.SuccessFul();
        }

        public void ConnectDeviceToServer(int port)
        {
            throw new NotImplementedException();
        }

        public Result ConnectionStatus(BaseDeviceInfoDto baseDeviceInfo)
        {
            if(ConnectToDevice(baseDeviceInfo.Ip, baseDeviceInfo.Port).Data)
            {
                _zksdk.SetConnectState(true);

                if (!_zksdk.GetConnectState())
                {
                    var error = ErrorDescriptor.GetError(-1, Zk);

                    return Result.Failed(error.Message);
                }
            }

            return Result.SuccessFul();
        }


        public Result DeactivateImage(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result DeleteAllUser(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device

            Log.Information($"Try To Delete All User From Device {baseDeviceInfo.Serial}");
            if (!_zksdk.ClearData(baseDeviceInfo.MachineNumber, 5))
            {
                Log.Error($"Error can not delete all User from device {baseDeviceInfo.Serial}");
                var error = ErrorDescriptor.GetError(-7, Zk);
                return Result.Failed(error.Message);
            }
            return Result.SuccessFul();
        }

        public Result DeleteUser(BaseDeviceInfoDto baseDeviceInfo, string personCode)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device

            if (!_zksdk.DeleteUserInfoEx(baseDeviceInfo.MachineNumber, Int32.Parse(personCode)))
            {
                Log.Error($"Error device can not delete user from device");
                return Result.SuccessFul();
            }

            Log.Information($"delete user from device successfully done {personCode}");

            return Result.SuccessFul();
        }

        public Result Disconnect(BaseDeviceInfoDto baseDeviceInfo)
        {
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);
                return Result.Failed(error.Message);
            }
            _zksdk.Disconnect();

            return Result.SuccessFul();
        }

        public Result EnrollUser(BaseDeviceInfoDto baseDeviceInfo, UserDto user)
        {
            throw new NotImplementedException();
        }

        public Result FactoryReset(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result<List<AccessGroupDto>> GetAccessGroupList(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result<List<AccessLevelDto>> GetAccessLevel(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result<List<UserDto>> GetAllUser(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result<List<UserDto>>.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device

            try
            {
                switch (baseDeviceInfo.Code)
                {
                    case DeviceModelTypes.uFace202:
                    case DeviceModelTypes.uFace202Plus:
                        return GetDeviceUserInfo.GetUsersInfo(baseDeviceInfo);
                    default:
                        return Result<List<UserDto>>.Failed(ResponseMessage.DeviceModelNotFound);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
                return Result<List<UserDto>>.Failed(ResponseMessage.UnHandleError);
            }
        }

        public Result<AuthConfigDto> GetAuthConfig(BaseDeviceInfoDto baseDeviceInfoDto)
        {
            throw new NotImplementedException();
        }

        public Result<List<DoorDto>> GetDoorList(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result<List<DoorStatusDto>> GetDoorStatus(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds)
        {
            throw new NotImplementedException();
        }

        public Result<List<HolidayGroupDto>> GetHolidayGroups(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<LogInfo>>> GetLogs(FilteredDeviceLogDto filteredDeviceLog)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(filteredDeviceLog.Ip, filteredDeviceLog.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Task.FromResult(Result<List<LogInfo>>.Failed(error.Message));
            }
            #endregion Check Connection Status Of Device 

            List<LogInfo> logInfos = new();

            try
            {
                switch (filteredDeviceLog.Code)
                {
                    case DeviceModelTypes.uFace202:
                    case DeviceModelTypes.uFace202Plus:
                        logInfos = ReadLog(filteredDeviceLog);
                        break;
                }
                return Task.FromResult(Result<List<LogInfo>>.SuccessFul(logInfos));
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
                throw;
            }
        }
        public Result<NetworkInfoDto> GetNetWorkConfig(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result<NetworkInfoDto>.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device 

            string IPAddr = "";
            string serialNum = "";

            if (!_zksdk.GetDeviceIP(baseDeviceInfo.MachineNumber, ref IPAddr) &&
             !_zksdk.GetSerialNumber(baseDeviceInfo.MachineNumber, out serialNum))
                Log.Error($"Error: Can not Read Config from Device{baseDeviceInfo.Serial}");

            var network = new NetworkInfoDto
            {
                Ip = IPAddr,
                Serial = serialNum,
                ServerAddress = IPAddr
            };
            return Result<NetworkInfoDto>.SuccessFul(network);
        }

        public Result<List<ScheduleDto>> GetScheduleList(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result<DateTime> GetTime(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result<DateTime>.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device         }

            int machineNumber = 1;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;

            if (!_zksdk.GetDeviceTime(machineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond))
                Log.Information($"error code : Can not get the time");

            return Result<DateTime>.Failed(ResponseMessage.CanNotReadTheTime);

            DateTime deviceTime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);

            return Result<DateTime>.SuccessFul(deviceTime);
        }
        public Result<UserDto> GetUserInfo(BaseDeviceInfoDto baseDeviceInfo, string personCode)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result<UserDto>.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device         }

            try
            {
                switch (baseDeviceInfo.Code)
                {
                    case DeviceModelTypes.uFace202:
                    case DeviceModelTypes.uFace202Plus:
                        return GetDeviceUserInfo.GetUserInfo(baseDeviceInfo, personCode);
                    default:
                        return Result<UserDto>.Failed(ResponseMessage.DeviceModelNotFound);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
                return Result<UserDto>.Failed(ResponseMessage.UnHandleError);
            }
        }

        public Result LockDevice(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result LockDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag)
        {
            throw new NotImplementedException();
        }

        public Result<int> LogCount(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result<int>.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device     

            try
            {
                int count = 0;
                if (!_zksdk.GetDeviceStatus(baseDeviceInfo.MachineNumber, 6, ref count))
                {
                    Log.Error("Can not Read Log Count");
                    var error = ErrorDescriptor.GetError(-1, Zk);
                    return Result<int>.Failed(error.Message);
                }
                return Result<int>.SuccessFul(count);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
                var error = ErrorDescriptor.GetError(-1, Zk);
                return Result<int>.Failed(error.Message);
            }

        }

        public Result RebootDevice(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device     

            if (!_zksdk.RestartDevice(baseDeviceInfo.MachineNumber))
            {
                Log.Error("Can not reboot device");
                var error = ErrorDescriptor.GetError(-1, Zk);
                return Result.Failed(error.Message);
            }
            return Result.SuccessFul();
        }

        public Result ReleaseDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag)
        {
            throw new NotImplementedException();
        }

        public Result RemoveAccessGroup(BaseDeviceInfoDto baseDeviceInfo, List<uint> accessGroupId)
        {
            throw new NotImplementedException();
        }

        public Result RemoveAccessLevel(BaseDeviceInfoDto baseDeviceInfo, List<uint> accessLevelIds)
        {
            throw new NotImplementedException();
        }

        public Result RemoveDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds)
        {
            throw new NotImplementedException();
        }

        public Result RemoveHolidayGroup(BaseDeviceInfoDto baseDeviceInfo, List<uint> holidayGroupId)
        {
            throw new NotImplementedException();
        }

        public Result RemoveSchedule(BaseDeviceInfoDto baseDeviceInfo, List<uint> scheduleIds)
        {
            throw new NotImplementedException();
        }

        public Result<ScanResultDto> ScanCard(BaseDeviceInfoDto baseDeviceInfo)
        {
             var result = _zksdk.ScanCard(baseDeviceInfo.MachineNumber);

            return Result<ScanResultDto>.SuccessFul(new ScanResultDto
            {
                Template = result.Template,
                TemplateData = result.TemplateData,
                TemplateImage = result.TemplateImage
            });
        }

        public Result<ScanResultDto> ScanFace(BaseDeviceInfoDto baseDeviceInfo)
        {
           throw new NotImplementedException();
        }
        public Result<ScanResultDto> ScanFinger(BaseDeviceInfoDto baseDeviceInfo, int quality)
        {
            var result = _zksdk.ScanFinger(baseDeviceInfo.MachineNumber);

            return Result<ScanResultDto>.SuccessFul(new ScanResultDto
            {
                Quality = result.Quality,
                Template = result.Template,
                TemplateData = result.TemplateData,
                TemplateImage = result.TemplateImage
            });
        }

        public DeviceInfo Search(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result SetAccessGroup(BaseDeviceInfoDto baseDeviceInfo, List<CreateAccessGroupDto> createAccessGroup)
        {
            throw new NotImplementedException();
        }

        public Result SetAccessLevel(BaseDeviceInfoDto baseDeviceInfo, List<CreateAccessLevelDto> createAccessLevels)
        {
            throw new NotImplementedException();
        }

        public Result SetAlarm(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorAlarmFlagEnum doorAlarmFlag)
        {
            throw new NotImplementedException();
        }

        public Result SetAuthConfig(BaseDeviceInfoDto baseDeviceInfoDto, SetAuthConfigDto setAuthConfigDto)
        {
            throw new NotImplementedException();
        }

        public Result SetDoor(BaseDeviceInfoDto baseDeviceInfo, CreateDoorDto createDoor)
        {
            throw new NotImplementedException();
        }

        public Result SetHolidayGroup(BaseDeviceInfoDto baseDeviceInfo, List<HolidayGroupDto> holidayGroup)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device     

            foreach (var holiyDay in holidayGroup)
            {
                if (!_zksdk.SSR_SetHoliday(baseDeviceInfo.MachineNumber, (int)holiyDay.Id, holiyDay.BeginMonth, holiyDay.BeginDay, holiyDay.EndMonth, holiyDay.EndDay, holiyDay.Timezone))
                    Log.Error($"Error can not set holiday group for device {baseDeviceInfo.Serial}");
                return Result.Failed(ResponseMessage.CannotSetHolidayGroup);
            }

            return Result.SuccessFul();
        }

        public Result<NetworkInfoDto> SetNetworkConfig(BaseDeviceInfoDto baseDeviceInfo, NetworkInfoDto networkInfo)
        {
            throw new NotImplementedException();
        }

        public Result SetSchedule(BaseDeviceInfoDto baseDeviceInfo, List<CreateScheduleDto> createSchedules)
        {
            throw new NotImplementedException();
        }

        public Result<bool> SetTime(BaseDeviceInfoDto baseDeviceInfo, DateTime date)
        {
            #region Check Connection Status Of Device
            var connection = _connectionStatus(baseDeviceInfo.Ip, baseDeviceInfo.Port);
            if (connection.Success == false)
            {
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result<bool>.Failed(error.Message);
            }
            #endregion Check Connection Status Of Device    

            if (!_zksdk.SetDeviceTime2(1, date.Year, date.Month,
                  date.Day, date.Hour, date.Minute, date.Second))
            {
                Log.Information($"error code : Can not set the time");
                var error = ErrorDescriptor.GetError(-1, Zk);

                return Result<bool>.Failed(error.Message);
            }

            return Result<bool>.SuccessFul(true);
        }

        public Result UnlockDevice(BaseDeviceInfoDto baseDeviceInfo)
        {
            throw new NotImplementedException();
        }

        public Result UnlockDoor(BaseDeviceInfoDto baseDeviceInfo, List<int> doorIds, DoorFlagEnum doorFlag)
        {
            throw new NotImplementedException();
        }

        #region connect
        public Result<bool> ConnectToDevice(string ip, int port)
        {
            if (!_zksdk.Connect_Net(ip, port))
            {
                int errorCode = 0;
                _zksdk.GetLastError(ref errorCode);
                Log.Error($"error code: {errorCode} : can not Open Socket to device");
                _zksdk.SetConnectState(false);
                return Result<bool>.Failed(ResponseMessage.DeviceCanNotConnect);
            }
            _zksdk.SetConnectState(true);
            return Result<bool>.SuccessFul(true);
        }
        #endregion

        #region ReadLog
        private List<LogInfo> ReadLog(FilteredDeviceLogDto filteredDeviceLog)
        {
            List<LogInfo> logRecord = new List<LogInfo>();

            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;

            int iFlag;
            string sTmpData = string.Empty;
            int iTmpLength;
            int idwFingerIndex;
            int iGLCount = 0;
            int iIndex = 0;

            int iFaceIndex = 50;//the only possible parameter value
            int iLength = 128 * 1024;//initialize the length(cannot be zero)
            byte[] byTmpData = new byte[iLength];

            try
            {
                _zksdk.EnableDevice(filteredDeviceLog.MachineNumber, false);
                if (_zksdk.ReadGeneralLogData(filteredDeviceLog.MachineNumber))//read all the attendance records to the memory
                {
                    while (_zksdk.SSR_GetGeneralLogData(filteredDeviceLog.MachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                                               out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {
                        for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                        {
                            if (_zksdk.GetUserTmpExStr(filteredDeviceLog.MachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength)
                                   ||
                               _zksdk.GetUserFace(filteredDeviceLog.MachineNumber, sdwEnrollNumber, iFaceIndex, ref byTmpData[0], ref iLength))
                            {
                                string returnValue = string.Empty;
                                _zksdk.GetSerialNumber(filteredDeviceLog.MachineNumber, out returnValue);
                                var log = new LogInfo
                                {
                                    AttendanceType = (AttendanceType)idwVerifyMode,
                                    Code = "",
                                    DeviceSerial = returnValue,
                                    EventTime = DateTime.Parse(idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString()),
                                    FunctionKey = new byte(),
                                    UserId = sdwEnrollNumber,
                                    //Image = sTmpData != string.Empty ? Encoding.ASCII.GetBytes(sTmpData)[0] : byTmpData[0],
                                    Image = new byte(),
                                    ImagePath = ""

                                };
                                logRecord.Add(log);
                            }
                        }
                    }
                }
                else
                {
                    Log.Error("Cannot read log count");
                    throw new Exception("Cannot read log count");
                }
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
            }
            return logRecord;
        }

        #endregion
    }
}