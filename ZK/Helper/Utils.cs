using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZK.Helper
{
    public class Utils
    {

        #region Date time

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime ConvertFromUnixToTimeStamp(uint time) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(time);

        #endregion Date time

        #region Mapping User Info

        public static UserDto Mapping(int idwFingerIndex, string sdwEnrollNumber, string sTmpData, int iFlag, int iPrivilege, string sName, int verifyStyle)
        {
            List<TemplateDto> userTemplates = new List<TemplateDto>();
            var user = new UserDto
            {
                //  StartDate = ConvertFromUnixToTimeStamp((uint)userHdr.startTime),
                //  EndDate = ConvertFromUnixToTimeStamp((uint)userHdr.expiryTime),
                Code = sdwEnrollNumber,
                OperatorLevel = (OperatorLevel)iPrivilege,
                AuthMode = GetAuthMode(verifyStyle),
            Name = sName,
            };

        var template = new TemplateDto
        {
            FingerIndex = (FingerIndex)idwFingerIndex,
            Image = null,
            Template = Encoding.ASCII.GetBytes(sTmpData),
            TemplateData = sTmpData,
            TemplateType = (TemplateType)iFlag
        };
        userTemplates.Add(template);

            user.Templates = userTemplates;

            return user;
        }

    #endregion Mapping User Info

    #region AutMode
    public static AuthMode GetAuthMode(int verifyCode)
    {
        //            128(FP / PW / RF), 129(FP), 130(PIN), 131(PW), 132(RF), 133(FP & RF), 134(FP / PW), 
        //135(FP / RF), 136(PW / RF), 137(PIN & FP), 138(FP & PW), 139(PW & RF), 140(FP & PW & RF), 
        //141(PIN & FP & PW), 142(FP & RF / PIN).
        switch (verifyCode)
        {
            case 132:
                return AuthMode.CardOnly;
            case 0:
                return AuthMode.Disable;
            case 138:
                return AuthMode.FingerAndPassword;
            case 129:
                return AuthMode.FingerOnly;
            case 134:
                return AuthMode.FingerOrPassword;
            case 131:
                return AuthMode.PasswordOnly;
            default:
                break;
        }

        return AuthMode.Disable;
    }
    #endregion
}
}