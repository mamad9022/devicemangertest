using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.UpdateConnectionCommand
{
    public class UpdateConnectionCommand :  IRequest<Result>
    {
        public long Id { get; set; }
    }
}