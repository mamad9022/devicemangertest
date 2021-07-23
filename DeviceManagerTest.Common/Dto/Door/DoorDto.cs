namespace Sepid.DeviceManagerTest.Common.Dto.Door
{
    public class DoorDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string EntryDeviceSerial { get; set; }
        public string ExitDeviceSerial { get; set; }

        public string RelayDeviceSerial { get; set; }
        public uint RelayPort { get; set; }

        public string ExitButtonDeviceSerial { get; set; }
        public uint ExitButtonPort { get; set; }
        public uint AutoLockTimeOut { get; set; }//ms
        public uint HeldOpenTimeout { get; set; }//ms
        public uint DualAuthScheduleId { get; set; }
        public uint DualAuthTimeout { get; set; }

        public byte UnlockFlags { get; set; }
        public byte LockFlags { get; set; }
        public byte DualAuthDevice { get; set; }
        public byte DualAuthApprovalType { get; set; }
        public byte UnconditionalLock { get; set; }
    }
}