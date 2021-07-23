using System.Collections.Generic;
using Sepid.DeviceManagerTest.Common.Enum;

namespace Sepid.DeviceManagerTest.Common.Dto.AuthConfig
{
    public class SetAuthConfigDto
    {
        public List<AuthConfigType> AuthConfigIds { get; set; }
        public int MatchTimeout { get; set; }
        public int AuthTimeout { get; set; }
    }
}