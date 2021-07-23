using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Schedules.Dto
{
    public class CreateHolidayGroupDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public List<CreateHolidayDto> Holidays { get; set; }
    }
}