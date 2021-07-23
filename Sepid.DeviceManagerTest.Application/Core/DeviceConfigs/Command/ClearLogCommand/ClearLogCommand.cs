using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.ClearLogCommand
{
    public class ClearLogCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
    }
}