using System;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class DeviceTo
    {
        public long Id { get; set; }
        public long BackUpLogMonth { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string Port { get; set; }
        public string Gateway { get; set; }
        public string SubnetMask { get; set; }
        public bool IsActive { get; set; }
        public bool UseDhcp { get; set; }
        public bool IsMatchOnServer { get; set; }
        public bool IsConnected { get; set; }

        public string SyncLogPeriod { get; set; }
        public DateTime SyncLogStartDate { get; set; }
        public DateTime? LastLogRetrieve { get; set; }
        public DeviceModelDto DeviceModel { get; set; }
    }
    
    public class DeviceModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DeviceModelTypes Code { get; set; }
        public SdkType SdkType { get; set; }
        public bool IsFingerSupport { get; set; }
        public bool IsFaceSupport { get; set; }
        public bool IsCardSupport { get; set; }
        public bool IsPasswordSupport { get; set; }
        public int TotalLog { get; set; }
    }
    
    public enum SdkType
    {
        BioStarV1 = 1,
        BioStarV2 = 2
    }
    
    
    public enum DeviceModelTypes
    {
        Undefined = 0,
        B1BioStation = 500,
        B1BioStationT2 = 501,
        B1BioentryPlus = 502,
        B1Biolite = 503,
        B1Xpass = 504,
        B1Dstation = 505,
        B1Xstation = 506,
        B1XpassSlim = 508,
        B1Fstation = 509,
        B1BioentryW = 510,
        B1XpassSlim2 = 511,

        B1BioStationT2Mellat = 515,

        B2BioStationL2 = 1000,
        B2BioStationA2 = 1001,
        B2BioStation2 = 1002,
        B2FaceStation2 = 1003,
        B2BioEntryR2 = 1004,
        B2BioEntryP2 = 1005,
        B2BioEntryW2 = 1006,
        B2BioLiteNet2 = 1007,
        B2Xpass2 = 1008,
        B2FaceLite = 1009,
        B2F2 = 8003
    }
}