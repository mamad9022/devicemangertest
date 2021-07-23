using MediatR;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.Groups.Command.UpdateDeviceGroup
{
    public class UpdateDeviceGroupCommand : IRequest<Result>
    {
        public long GroupId { get; set; }

        public List<int> DeviceIds { get; set; }
    }
}
