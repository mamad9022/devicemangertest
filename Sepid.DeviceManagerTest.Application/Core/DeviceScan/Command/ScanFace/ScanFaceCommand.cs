using MediatR;
using Sepid.DeviceManagerTest.Application.Core.DeviceScan.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceScan.Command.ScanFace
{
    public class ScanFaceCommand : IRequest<Result<TemplateDataDto>>
    {
        public int DeviceId { get; set; }
    }
}