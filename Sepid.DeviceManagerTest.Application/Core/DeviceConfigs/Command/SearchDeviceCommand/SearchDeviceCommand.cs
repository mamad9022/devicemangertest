using MediatR;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Dto;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SearchDeviceCommand
{
    public class SearchDeviceCommand : IRequest<DeviceSearchDto>
    {
        public string Ip { get; set; }
        public int Port { get; set; } = 0;
    }
}