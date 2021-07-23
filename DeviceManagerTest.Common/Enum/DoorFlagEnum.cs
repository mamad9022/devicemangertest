namespace Sepid.DeviceManagerTest.Common.Enum
{
    public enum DoorFlagEnum
    {
        None = 0,
        Schedule = 1,
        Emergency = 2,
        Operator = 4
    }

    public enum DoorAlarmFlagEnum
    {
        None = 0,
        ForcedOpen = 1,
        HeldOpen = 2,
        Apb = 4
    }
}