using System;

namespace Sepid.DeviceManagerTest.Application.Core.Schedules.Dto
{
    public class CreateHolidayDto
    {
        public DateTime Date { get; set; }
        public byte Recurrence { get; set; }
    }
}