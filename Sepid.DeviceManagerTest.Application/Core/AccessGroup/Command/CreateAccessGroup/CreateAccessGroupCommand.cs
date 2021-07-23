using MediatR;
using Sepid.DeviceManagerTest.Common.Dto.AccessControl;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;

namespace Sepid.DeviceManagerTest.Application.Core.AccessGroup.Command.CreateAccessGroup
{
    public class CreateAccessGroupCommand : IRequest<Result>
    {
        public string DeviceSerial { get; set; }

        public List<CreateAccessGroupDto> CreateAccessGroup { get; set; }
    }
}