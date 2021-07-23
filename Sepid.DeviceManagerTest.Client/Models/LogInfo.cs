using System;

namespace Sepid.DeviceManagerTest.Client.Models
{
    public class LogInfo
    {
        public uint? Id { get; set; }
        public DateTime EventTime { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public string DeviceSerial { get; set; }
        public byte Image { get; set; }
        public byte FunctionKey { get; set; }
        public AttendanceType AttendanceType { get; set; }
    }

    public enum AttendanceType
    {
        Undefined = 0,
        Finger = 1,
        Card = 2,
        Face = 3,
        Pin = 4,
        Id = 5,
    }
}