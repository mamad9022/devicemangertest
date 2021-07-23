using System.Text;
using ZK.Dto.Scan;

namespace ZK.Common
{
    public class Zksdk : IZksdk
    {
        public zkemkeeper.CZKEMClass objCZKEM = new zkemkeeper.CZKEMClass();
        private static bool bIsConnected = false;//the boolean value identifies whether the device is connected

        public bool ClearData(int dwMachineNumber, int DataFlag)
        {
            return objCZKEM.ClearData(dwMachineNumber, DataFlag);
        }

        public bool ClearLog(int dwMachineNumber)
        {
            return objCZKEM.ClearGLog(dwMachineNumber);
        }

        public bool Connect_Net(string IPAdd, int Port)
        {
            return objCZKEM.Connect_Net(IPAdd, Port);
        }

        public bool DeleteUserInfoEx(int dwMachineNumber, int dwEnrollNumber)
        {
            return objCZKEM.DeleteUserInfoEx(dwMachineNumber, dwEnrollNumber);
        }

        public void Disconnect()
        {
            objCZKEM.Disconnect();
        }

        public bool EnableDevice(int dwMachineNumber, bool bFlag)
        {
            return objCZKEM.EnableDevice(dwMachineNumber, bFlag);
        }

        public bool GetDeviceIP(int dwMachineNumber, ref string IPAddr)
        {
            return objCZKEM.GetDeviceIP(dwMachineNumber, ref IPAddr);
        }

        public bool GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue)
        {
            return objCZKEM.GetDeviceStatus(dwMachineNumber, dwStatus, ref dwValue);
        }

        public bool GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
        {
            return objCZKEM.GetDeviceTime(dwMachineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
        }

        public bool GetUserFace(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, ref byte TmpData, ref int TmpLength)
        {
            return objCZKEM.GetUserFace(dwMachineNumber, dwEnrollNumber, dwFaceIndex, ref TmpData, ref TmpLength);
        }

        public bool GetUserTmpExStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out int Flag, out string TmpData, out int TmpLength)
        {
            return objCZKEM.GetUserTmpExStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out Flag, out TmpData, out TmpLength);
        }

        public bool GetSerialNumber(int dwMachineNumber, out string dwSerialNumber)
        {
            return objCZKEM.GetSerialNumber(dwMachineNumber, out dwSerialNumber);
        }

        public bool ReadAllTemplate(int dwMachineNumber)
        {
            return objCZKEM.ReadAllTemplate(dwMachineNumber);
        }

        public bool ReadAllUserID(int dwMachineNumber)
        {
            return objCZKEM.ReadAllUserID(dwMachineNumber);
        }

        public bool ReadGeneralLogData(int dwMachineNumber)
        {
            return objCZKEM.ReadGeneralLogData(dwMachineNumber);
        }

        public bool RefreshData(int dwMachineNumber)
        {
            return objCZKEM.RefreshData(dwMachineNumber);
        }

        public bool RestartDevice(int dwMachineNumber)
        {
            return objCZKEM.RestartDevice(dwMachineNumber);
        }

        public bool SetDeviceTime2(int dwMachineNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond)
        {
            return objCZKEM.SetDeviceTime2(dwMachineNumber, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
        }

        public bool SSR_GetAllUserInfo(int dwMachineNumber, out string dwEnrollNumber, out string Name, out string Password, out int Privilege, out bool Enabled)
        {
            return objCZKEM.SSR_GetAllUserInfo(dwMachineNumber, out dwEnrollNumber, out Name, out Password, out Privilege, out Enabled);
        }

        public bool SSR_GetGeneralLogData(int dwMachineNumber, out string dwEnrollNumber, out int dwVerifyMode, out int dwInOutMode, out int dwYear, out int dwMonth, out int dwDay, out int dwHour, out int dwMinute, out int dwSecond, ref int dwWorkCode)
        {
            return objCZKEM.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode);
        }

        public bool SSR_GetUserInfo(int dwMachineNumber, string dwEnrollNumber, out string Name, out string Password, out int Privilege, out bool Enabled)
        {
            return objCZKEM.SSR_GetUserInfo(dwMachineNumber, dwEnrollNumber, out Name, out Password, out Privilege, out Enabled);
        }

        public bool SSR_SetHoliday(int dwMachineNumber, int HolidayID, int BeginMonth, int BeginDay, int EndMonth, int EndDay, int TimeZoneID)
        {
            return objCZKEM.SSR_SetHoliday(dwMachineNumber, HolidayID, BeginMonth, BeginDay, EndMonth, EndDay, TimeZoneID);
        }

        public bool SSR_SetUserInfo(int dwMachineNumber, string dwEnrollNumber, string Name, string Password, int Privilege, bool Enabled)
        {
            return objCZKEM.SSR_SetUserInfo(dwMachineNumber, dwEnrollNumber, Name, Password, Privilege, Enabled);
        }

        public bool StartEnrollEx(string UserID, int FingerID, int Flag)
        {
            return objCZKEM.StartEnrollEx(UserID, FingerID, Flag);
        }

        public void GetLastError(ref int idwErrorCode)
        {
            objCZKEM.GetLastError(ref idwErrorCode);
        }

        public ScanDto ScanFinger(int dwMachineNumber)
        {
            int Flag = 0;
            string TmpData;
            int TmpLength;

            ScanDto scanFinger = null;
            if (objCZKEM.RegEvent(dwMachineNumber, 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                objCZKEM.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
            }

            #region When you have enrolled your finger,this event will be triggered and return the quality of the fingerprint you have enrolled
            void axCZKEM1_OnFingerFeature(int Score)
            {
                scanFinger.Quality = Score;
                for (int idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (GetUserTmpExStr(dwMachineNumber,scanFinger.UserId.ToString(),idwFingerIndex,out Flag, out TmpData, out TmpLength))
                    {
                        scanFinger.Template = TmpData;
                        scanFinger.TemplateData = Encoding.ASCII.GetBytes(TmpData);
                        scanFinger.TemplateImage = Encoding.ASCII.GetBytes(TmpData);
                    }
                }
            }
            #endregion

            #region After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered. If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
            void axCZKEM1_OnVerify(int UserID)
            {
                if (UserID != -1)
                {
                    scanFinger.UserId = UserID;
                    objCZKEM.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                }
                else
                {
                }

            }
            #endregion

            #region When you place your finger on sensor of the device,this event will be triggered
            void axCZKEM1_OnFinger()
            {
                scanFinger = new ScanDto();
                objCZKEM.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
            }
            #endregion
           
            return scanFinger;
        }

        public ScanDto ScanCard(int dwMachineNumber)
        {
            int Flag = 0;
            string TmpData;
            int TmpLength;
            string strHIDEventCardNum;

            ScanDto scanCard = null;
            if (objCZKEM.RegEvent(dwMachineNumber, 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                objCZKEM.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
            }

            #region After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered. If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
            void axCZKEM1_OnVerify(int UserID)
            {
                if (UserID != -1)
                {
                    scanCard.UserId = UserID;
                    for (int idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                    {
                        if (GetUserTmpExStr(dwMachineNumber, scanCard.UserId.ToString(), idwFingerIndex, out Flag, out TmpData, out TmpLength))
                        {
                            objCZKEM.GetHIDEventCardNumAsStr(out strHIDEventCardNum);

                            scanCard.Template = TmpData;
                            scanCard.TemplateData = Encoding.ASCII.GetBytes(TmpData);
                            scanCard.TemplateImage = Encoding.ASCII.GetBytes(TmpData);
                            scanCard.CardNumber = strHIDEventCardNum;
                        }
                    }
                }
                else
                {
                }

            }
            #endregion

            return scanCard;
        }

        public void SetConnectState(bool state)
        {
            bIsConnected = state;
        }

        public bool GetConnectState()
        {
            return bIsConnected;
        }

        public bool GetUserInfoEx(int dwMachineNumber, int dwEnrollNumber, out int VerifyStyle, out byte Reserved)
        {
             return objCZKEM.GetUserInfoEx(dwMachineNumber, dwEnrollNumber, out VerifyStyle, out Reserved);
        }

        public bool GetUserInfoByCard(int dwMachineNumber, ref string Name, ref string Password, ref int Privilege, ref bool Enabled)
        {
            return objCZKEM.GetUserInfoByCard(dwMachineNumber, ref Name, ref Password, ref Privilege, ref Enabled);
        }
    }
}
