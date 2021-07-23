using MediatR;
using Sepid.DeviceManagerTest.Common.Results;


namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.ActiveImageCommand
{
    public class ActivateImageLogCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
    }
}