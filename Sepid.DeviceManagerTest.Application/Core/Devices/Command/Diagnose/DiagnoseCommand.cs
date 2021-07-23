using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Command.Diagnose
{
    public class DiagnoseCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
    }
}