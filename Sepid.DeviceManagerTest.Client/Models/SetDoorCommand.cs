namespace Sepid.DeviceManagerTest.Client.Models
{
    public class SetDoorCommand
    {
        public string DeviceSerial { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string EntryDeviceSerial { get; set; } = "0";
        public string ExitDeviceSerial { get; set; } = "0";
        public uint AutoLockTimeout { get; set; } = 3;
        public uint HeldOpenTimeout { get; set; } = 3;
        public bool InstantLock { get; set; }
        public bool HasRelay { get; set; }
        public string RelayDeviceSerial { get; set; }
        public uint Port { get; set; }
        public bool HasSensor { get; set; }
        public string SensorDeviceSerial { get; set; }
        public uint SensorPort { get; set; }
        public bool HasButton { get; set; }
        public string ButtonDeviceSerial { get; set; }
        public uint ButtonPort { get; set; }
        public bool UnConditionalLock { get; set; }
    }
}