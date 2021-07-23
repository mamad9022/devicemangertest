using System.Threading.Tasks;
using Refit;
using Sepid.DeviceManagerTest.Client.Models;

namespace Sepid.DeviceManagerTest.Client.Interfaces
{
    public interface IDoorsApi
    {
        [Post("/Doors")]
        Task<ApiResponse<Result.Result>> CreateDoor([Body] SetDoorCommand setDoorCommand,[Header("Authorization")] string token);

        [Put("/Doors/Remove")]
        Task<ApiResponse<Result.Result>> RemoveDoor([Body] RemoveDoorCommand removeDoorCommand,[Header("Authorization")] string token);
    }
}