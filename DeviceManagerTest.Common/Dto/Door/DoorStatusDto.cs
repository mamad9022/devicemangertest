using Sepid.DeviceManagerTest.Common.Enum;
using System;

namespace Sepid.DeviceManagerTest.Common.Dto.Door
{
    public class DoorStatusDto
    {
        public uint Id { get; set; }
        public bool Opened { get; set; }
        public bool Unlocked { get; set; }
        public bool HeldOpened { get; set; }
        public DoorFlagEnum UnlockFlags { get; set; }
        public DoorFlagEnum LockFlags { get; set; }
        public DoorAlarmFlagEnum AlarmFlags { get; set; }
        public DateTime LastOpenTime { get; set; }
    }
}