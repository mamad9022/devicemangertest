using MediatR;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.AccessGroup.Command.RemoveAccessLevel
{
    public class RemoveAccessLevelCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }

        public List<uint> AccessLevelId { get; set; }
    }
}