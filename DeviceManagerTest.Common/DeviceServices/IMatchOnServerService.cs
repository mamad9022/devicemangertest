using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Common.DeviceServices
{
    public interface IMatchOnServerService
    {
        Result ServerMatching(BaseDeviceInfoDto baseDeviceInfoDto);

        Result DetachServerMatching(BaseDeviceInfoDto baseDeviceInfoDto);
    }
}