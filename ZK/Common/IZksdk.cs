using ZK.Dto.Scan;

namespace ZK.Common
{
    public interface IZksdk
    {
        public bool Connect_Net(string IPAdd, int Port);
        public void SetConnectState(bool state);
        public bool GetConnectState();
        public bool GetUserInfoByCard(int dwMachineNumber, ref string Name, ref string Password, ref int Privilege, ref bool Enabled);
        public bool ClearLog(int dwMachineNumber);
        public bool ClearData(int dwMachineNumber, int DataFlag);
        public bool RefreshData(int dwMachineNumber);
        public void GetLastError(ref int idwErrorCode);
        public bool DeleteUserInfoEx(int dwMachineNumber, int dwEnrollNumber);
        public void Disconnect();
        public bool GetUserInfoEx(int dwMachineNumber, int dwEnrollNumber, out int VerifyStyle, out byte Reserved);
        public bool SSR_GetUserInfo(int dwMachineNumber, string dwEnrollNumber, out string Name, out string Password, out int Privilege, out bool Enabled);
        public bool SSR_SetUserInfo(int dwMachineNumber, string dwEnrollNumber, string Name, string Password, int Privilege, bool Enabled);
        public bool StartEnrollEx(string UserID, int FingerID, int Flag);
        public bool ReadAllUserID(int dwMachineNumber);
        public bool ReadAllTemplate(int dwMachineNumber);
        public bool SSR_GetAllUserInfo(int dwMachineNumber, out string dwEnrollNumber, out string Name, out string Password, out int Privilege, out bool Enabled);
        public bool GetUserTmpExStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out int Flag, out string TmpData, out int TmpLength);
        public bool EnableDevice(int dwMachineNumber, bool bFlag);
        public bool ReadGeneralLogData(int dwMachineNumber);
        public bool SSR_GetGeneralLogData(int dwMachineNumber, out string dwEnrollNumber, out int dwVerifyMode, out int dwInOutMode, out int dwYear, out int dwMonth, out int dwDay, out int dwHour, out int dwMinute, out int dwSecond, ref int dwWorkCode);
        public bool GetUserFace(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, ref byte TmpData, ref int TmpLength);
        public bool GetSerialNumber(int dwMachineNumber, out string dwSerialNumber);
        public bool GetDeviceIP(int dwMachineNumber, ref string IPAddr);
        public bool GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);
        public bool GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue);
        public bool RestartDevice(int dwMachineNumber);
        public bool SSR_SetHoliday(int dwMachineNumber, int HolidayID, int BeginMonth, int BeginDay, int EndMonth, int EndDay, int TimeZoneID);
        public bool SetDeviceTime2(int dwMachineNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond);
        public ScanDto ScanFinger(int dwMachineNumber);
        public ScanDto ScanCard(int dwMachineNumber);


    }
}
