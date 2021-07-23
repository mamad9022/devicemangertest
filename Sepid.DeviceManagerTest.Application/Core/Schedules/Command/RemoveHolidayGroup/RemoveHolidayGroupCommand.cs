using MediatR;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Schedules.Command.RemoveHolidayGroup
{
    public class RemoveHolidayGroupCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }

        public List<uint> ScheduleGroupIds { get; set; }
    }
}