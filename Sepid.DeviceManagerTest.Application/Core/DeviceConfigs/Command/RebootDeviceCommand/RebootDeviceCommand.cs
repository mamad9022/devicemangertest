using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.RebootDeviceCommand
{
    public class RebootDeviceCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}