using MediatR;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.GetTimeCommand
{
    public class GetTimeCommand : IRequest<Result<DeviceDateDto>>
    {
        public int DeviceId { get; set; }
    }
}