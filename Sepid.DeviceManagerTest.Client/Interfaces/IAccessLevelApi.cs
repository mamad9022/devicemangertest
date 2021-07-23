using System.Threading.Tasks;
using Refit;
using Sepid.DeviceManagerTest.Client.Models;

namespace Sepid.DeviceManagerTest.Client.Interfaces
{
    public interface IAccessLevelApi
    {
        [Post("/Access/setAccessLevel")]
        Task<ApiResponse<Result.Result>> SetAccessLevel([Body] CreateAccessLevelCommand createAccessLevelCommand,[Header("Authorization")] string token);


        [Put("/Access/removeAccessLevel")]
        Task<ApiResponse<Result.Result>> RemoveAccessLevel([Body] RemoveAccessLevelCommand removeAccessLevelCommand,[Header("Authorization")] string token);
    }
}