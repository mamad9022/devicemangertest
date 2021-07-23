using MediatR;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanFinger
{
    public class ScanFingerCommand : IRequest<Result<FingerDto>>
    {
        public int DeviceId { get; set; }
    }
}