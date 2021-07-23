using MediatR;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanCard
{
    public class ScanCardCommand : IRequest<Result<TemplateDataDto>>
    {
        public int DeviceId { get; set; }
    }
}