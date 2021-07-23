using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Delete
{
    public class DeleteDeviceModelCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}