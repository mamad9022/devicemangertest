using MediatR;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.FactoryResetCommand
{
    public class FactoryResetCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}