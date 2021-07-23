using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Sepid.DeviceManagerTest.Client.Models;

namespace Sepid.DeviceManagerTest.Client.Interfaces
{
    public interface IDevicesApi
    {
        [Get("/Devices/List")]
        Task<ApiResponse<Result.Result<List<DeviceTo>>>> GetAllDevices([Header("Authorization")] string token);



    }
}