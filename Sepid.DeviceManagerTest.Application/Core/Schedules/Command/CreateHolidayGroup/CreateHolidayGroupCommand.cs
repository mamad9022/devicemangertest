using MediatR;
using Sepid.DeviceManagerTest.Application.Core.Schedules.Dto;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Schedules.Command.CreateHolidayGroup
{
    public class CreateHolidayGroupCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }

        public List<CreateHolidayGroupDto> CreateHolidayGroup { get; set; }
    }
}