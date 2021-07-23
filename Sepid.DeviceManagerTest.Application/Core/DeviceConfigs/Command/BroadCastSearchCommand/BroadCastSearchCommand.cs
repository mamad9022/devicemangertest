using MediatR;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Dto;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.BroadCastSearchCommand
{
    public class BroadCastSearchCommand : IRequest<List<DeviceSearchDto>>
    {
    }
}