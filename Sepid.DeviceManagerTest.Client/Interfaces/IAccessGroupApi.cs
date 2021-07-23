using System.Threading.Tasks;
using Refit;
using Sepid.DeviceManagerTest.Client.Models;

namespace Sepid.DeviceManagerTest.Client.Interfaces
{
    public interface IAccessGroupApi
    {
        [Post("/Access/SetAccessGroup")]
        Task<ApiResponse<Result.Result>> SetAccessGroup([Body] CreateAccessGroupCommand createAccessGroupCommand,[Header("Authorization")] string token);


        [Put("/Access/RemoveAccessGroup")]
        Task<ApiResponse<Result.Result>> RemoveAccessGroup([Body] RemoveAccessGroupCommand removeAccessGroupCommand,[Header("Authorization")] string token);
    }
}