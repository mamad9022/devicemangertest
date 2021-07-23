using MediatR;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Schedules.Command.RemoveSchedule
{
    public class RemoveScheduleCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }

        public List<uint> ScheduleIds { get; set; }
    }
}