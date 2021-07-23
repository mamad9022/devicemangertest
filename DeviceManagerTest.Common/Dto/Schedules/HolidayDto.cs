using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Common.Dto.Schedules
{
    public class HolidayGroupDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public List<HolidayDto> Holidays { get; set; }
        public int BeginMonth { get; set; }
        public int BeginDay { get; set; }
        public int EndMonth { get; set; }
        public int EndDay { get; set; }
        public int Timezone { get; set; }

    }

    public class HolidayDto
    {
        public DateTime Date { get; set; }
        public byte Recurrence { get; set; }
    }
}