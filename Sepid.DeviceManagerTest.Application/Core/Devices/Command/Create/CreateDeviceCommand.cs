using MediatR;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Common.Results;
using System;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Command.Create
{
    public class CreateDeviceCommand : IRequest<Result<DeviceDto>>
    {
        public long DeviceModelId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public string ServerIp { get; set; }
        public int? ServerPort { get; set; }
        public int Port { get; set; }
        public string Gateway { get; set; }
        public string SubnetMask { get; set; }
        public bool IsVital { get; set; }
        public bool UseDhcp { get; set; }
        public bool IsDeviceToServer { get; set; }
        public bool IsMatchOnServer { get; set; }
      
        public DateTime SyncLogStartDate { get; set; }
        public EntranceMode EntranceMode { get; set; }
    }
}