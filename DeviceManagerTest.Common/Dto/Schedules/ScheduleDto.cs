using System;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Common.Dto.Schedules
{
    public class ScheduleDto
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public bool IsDaily { get; set; }

        public byte NumHolidaySchedules { get; set; }

        public DailySchedule DailySchedule { get; set; }

        public WeeklySchedule WeeklySchedule { get; set; }

        public List<HolidayScheduleDto> Holidays { get; set; }
    }

    public class HolidayScheduleDto
    {
        public uint Id { get; set; }

        public List<DayScheduleDto> DaySchedules { get; set; }
    }

    public class DayScheduleDto
    {
        public byte NumPeriods { get; set; }

        public List<TimePeriodDto> TimePeriods { get; set; }
    }

    public class TimePeriodDto
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }

    public class DailySchedule
    {
        public DateTime StartDate { get; set; }

        public byte NumDays { get; set; }

        public List<TimePeriodDto> TimePeriods { get; set; }
    }

    public class WeeklySchedule
    {
        public List<DayScheduleDto> DaySchedules { get; set; }
    }
}