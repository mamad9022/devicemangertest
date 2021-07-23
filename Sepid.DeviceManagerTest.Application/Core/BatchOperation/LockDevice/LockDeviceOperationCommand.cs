using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.BatchOperation.LockDevice
{
    public class LockDeviceOperationCommand : IRequest<Result>
    {
        public List<long> GroupIds { get; set; }
    }
}