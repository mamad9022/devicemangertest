using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.LockDeviceCommand
{
    public class LockDeviceCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}