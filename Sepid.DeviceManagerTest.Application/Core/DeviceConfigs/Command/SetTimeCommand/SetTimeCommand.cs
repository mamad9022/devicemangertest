using MediatR;
using Sepid.DeviceManagerTest.Common.Results;
using System;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetTimeCommand
{
    public class SetTimeCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
        public DateTime Date { get; set; }
    }
}