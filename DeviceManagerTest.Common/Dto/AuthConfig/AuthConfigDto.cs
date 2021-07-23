using System.Collections.Generic;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Common.Dto.AuthConfig
{
    public class AuthConfigDto
    {
        public List<AuthConfigType> AuthConfigIds { get; set; }
        
        public byte MatchTimeout { get; set; }
        public byte AuthTimeout { get; set; }
    }
}