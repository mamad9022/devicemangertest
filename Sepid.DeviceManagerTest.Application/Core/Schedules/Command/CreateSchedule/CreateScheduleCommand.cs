using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Dto.Schedules;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Schedules.Command.CreateSchedule
{
    public class CreateScheduleCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }

        public List<CreateScheduleDto> CreateSchedules { get; set; }
    }
}