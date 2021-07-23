using System.Collections.Generic;
using Refit;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Client.Models;

namespace Sepid.DeviceManagerTest.Client.Interfaces
{
    public interface IUserEnrollmentApi
    {

        [Get("/Devices/GetAllUser/{deviceSerial}")]
        Task<ApiResponse<Result.Result<List<UseDto>>>> GetAllUser(string deviceSerial,[Header("Authorization")] string token);

        [Post("/Devices/EnrollUser")]
        Task<ApiResponse<Result.Result>> SendToDevice([Body]EnrollUserCommand enrollUserCommand,[Header("Authorization")] string token);

        [Delete("/Devices/DeleteUser")]
        Task<ApiResponse<Result.Result>> RemoveFromDevice([Query]DeleteUserCommand deleteUserCommand,[Header("Authorization")] string token);
    }
}