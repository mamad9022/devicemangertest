using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.UnlockDeviceCommand
{
    public class UnlockDeviceCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}