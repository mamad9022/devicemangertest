using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Command.Delete
{
    public class DeleteDeviceCommand : IRequest<Result>
    {
        public long Id { get; set; }
    }
}