namespace Sepid.DeviceManagerTest.Common.Dto
{
    public class NetworkInfoDto
    {
        public bool UseDhcp { get; set; }
        public bool UseDns { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public string ServerAddress { get; set; }
        public string Gateway { get; set; }
        public string SubnetMask { get; set; }
        public uint Port { get; set; }
        public uint ServerPort { get; set; }
        public bool IsDeviceToServer { get; set; }
        public bool IsMatchOnServer { get; set; }
    }
}