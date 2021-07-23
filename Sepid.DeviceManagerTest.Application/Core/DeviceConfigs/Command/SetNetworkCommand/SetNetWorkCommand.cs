using MediatR;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetNetworkCommand
{
    public class SetNetWorkCommand : IRequest<Result<NetworkInfoDto>>
    {
        public int DeviceId { get; set; }
        public bool UseDhcp { get; set; }
        public bool UseDns { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public string ServerAddress { get; set; }
        public string GateWay { get; set; }
        public string SubnetMask { get; set; }
        public uint Port { get; set; }
        public uint ServerPort { get; set; }
        public bool IsDeviceToServer { get; set; }
        public bool IsMatchOnServer { get; set; }
    }
}