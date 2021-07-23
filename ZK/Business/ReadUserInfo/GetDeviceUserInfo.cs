using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using Serilog;
using System;
using System.Collections.Generic;
using ZK.Common;

namespace ZK.Business.ReadUserInfo
{
    public static class GetDeviceUserInfo
    {
        private static IZksdk _zksdk = new Zksdk();
        public static Result<List<UserDto>> GetUsersInfo(BaseDeviceInfoDto baseDeviceInfo)
        {
            #region parameter
            string sdwEnrollNumber = string.Empty,
            sName = string.Empty,
            sPassword = string.Empty,
            sTmpData = string.Empty;
            int iPrivilege = 0,
            iTmpLength = 0,
            iFlag = 0,
            idwFingerIndex;
            bool bEnabled = false;
            int VerifyStyle;
            byte Reserved;
            #endregion parameter

            List<UserDto> users = new List<UserDto>();

            _zksdk.ReadAllUserID(baseDeviceInfo.MachineNumber);
            _zksdk.ReadAllTemplate(baseDeviceInfo.MachineNumber);

            try
            {
                while (_zksdk.SSR_GetAllUserInfo(baseDeviceInfo.MachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))
                {
                    _zksdk.GetUserInfoEx(baseDeviceInfo.MachineNumber, Int32.Parse(sdwEnrollNumber), out VerifyStyle, out Reserved);

                    for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                    {
                        if (_zksdk.GetUserTmpExStr(baseDeviceInfo.MachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))
                        {
                            users.Add(Helper.Utils.Mapping(idwFingerIndex, sdwEnrollNumber, sTmpData, iFlag, iPrivilege, sName, VerifyStyle));
                        }
                    }
                }
                return Result<List<UserDto>>.SuccessFul(users);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
                return Result<List<UserDto>>.Failed(ResponseMessage.CanNotGetUserFromDevice);
            }
        }
        public static Result<UserDto> GetUserInfo(BaseDeviceInfoDto baseDeviceInfo, string personCode)
        {
            #region parameter
            int iFaceIndex = 50;//the only possible parameter value
            int iLength = 128 * 1024;//initialize the length(cannot be zero)
            byte[] byTmpData = new byte[iLength];
            string sdwEnrollNumber = string.Empty,
            Name = string.Empty,
            Password = string.Empty,
            sTmpData = string.Empty;
            int Privilege = 0,
            iTmpLength = 0,
            iFlag = 0,
            idwFingerIndex = 6;
            bool Enabled = false;
            int VerifyStyle;
            byte Reserved;
            #endregion parameter

            _zksdk.ReadAllUserID(baseDeviceInfo.MachineNumber);
            _zksdk.ReadAllTemplate(baseDeviceInfo.MachineNumber);

            try
            {
                var user = new UserDto();

                if (_zksdk.SSR_GetUserInfo(baseDeviceInfo.MachineNumber, personCode, out Name, out Password, out Privilege, out Enabled))
                {
                    _zksdk.GetUserInfoEx(baseDeviceInfo.MachineNumber, Int32.Parse(personCode), out VerifyStyle, out Reserved);
                   
                    for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                    {
                        if (_zksdk.GetUserTmpExStr(baseDeviceInfo.MachineNumber, personCode, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength)
                               ||
                           _zksdk.GetUserFace(baseDeviceInfo.MachineNumber, personCode, iFaceIndex, ref byTmpData[0], ref iLength))
                        {
                            _zksdk.GetUserTmpExStr(baseDeviceInfo.MachineNumber, personCode, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength);
                            user = Helper.Utils.Mapping(idwFingerIndex, personCode, sTmpData, iFlag, Privilege, Name, VerifyStyle);
                        }
                    }
                }
                return Result<UserDto>.SuccessFul(user);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
                return Result<UserDto>.Failed(ResponseMessage.CanNotGetUserFromDevice);
            }
        }
    }
}