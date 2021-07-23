using MediatR;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Queries
{
    public class GetNetworkConfigDeviceQuery : IRequest<Result<NetworkInfoDto>>
    {
        public int DeviceId { get; set; }
    }
}