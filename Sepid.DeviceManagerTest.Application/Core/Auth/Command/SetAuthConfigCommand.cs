using System.Collections.Generic;
using MediatR;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Auth.Command
{
    public class SetAuthConfigCommand : IRequest<Result>
    {
        public int DeviceId { get; set; }
        public List<AuthConfigType> AuthConfigIds { get; set; }
        public int MatchTimeout { get; set; }
        public int AuthTimeout { get; set; }
    }
}