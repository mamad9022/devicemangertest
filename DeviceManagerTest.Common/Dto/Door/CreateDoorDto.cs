namespace Sepid.DeviceManagerTest.Common.Dto.Door
{
    public class CreateDoorDto
    {
        public string DeviceSerial { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string EntryDeviceSerial { get; set; } = "0";
        public string ExitDeviceSerial { get; set; } = "0";
        public uint AutoLockTimeout { get; set; } = 3;
        public uint HeldOpenTimeout { get; set; } = 3;
        public bool InstantLock { get; set; } = false;
        public bool HasRelay { get; set; } = false;
        public string RelayDeviceSerial { get; set; } = "0";
        public uint Port { get; set; } = 0;
        public bool HasSensor { get; set; } = false;
        public string SensorDeviceSerial { get; set; } = "0";
        public uint SensorPort { get; set; } = 0;
        public bool HasButton { get; set; } = false;
        public string ButtonDeviceSerial { get; set; } = "0";
        public uint ButtonPort { get; set; } = 0;
        public bool UnConditionalLock { get; set; } = false;
        //public bool HasDualAuthentication { get; set; } = false;
        //public uint DualAuthTimeout { get; set; } = 0;
        //public uint NumDualAuthApprovalGroups { get; set; } = 0;
        //public DualAuthDevice DualAuthDevice { get; set; } = DualAuthDevice.NoDevice;
        //public ScheduleIdEnum ShcScheduleIdEnum { get; set; } = ScheduleIdEnum.Never;
        //public DualAuthApprovalEnum DualAuthApproval { get; set; } = DualAuthApprovalEnum.None;
    }
}