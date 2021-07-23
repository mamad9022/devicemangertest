using Microsoft.AspNetCore.Mvc;

namespace Sepid.DeviceManagerTest.Common.Results
{
    public class Result<T>
    {
        public T Data { get; private set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public ObjectResult ApiResult { get; set; }
        public bool Success { get; set; }

        public static Result<T> SuccessFul(T data) => new Result<T> { ApiResult = new OkObjectResult(data), Data = data, Message = null, Success = true };

        public static Result<T> SuccessFul(ObjectResult success) => new Result<T> { ApiResult = success, Success = true };

        public static Result<T> Failed(ObjectResult error) => new Result<T> { ApiResult = error, Success = false, Message = error.Value?.ToString() };

        public static Result<T> Failed(string message, int statusCode = 400) => new Result<T> { Success = false, Message = message, StatusCode = statusCode };
    }

    public class Result
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string Reason { get; set; }

        public ObjectResult ApiResult { get; set; }
        public bool Success { get; set; }

        public static Result SuccessFul() => new Result { Message = null, Success = true };

        public static Result SuccessFul(ObjectResult success) => new Result { ApiResult = success, Success = true };

        public static Result Failed(ObjectResult error) => new Result { ApiResult = error, Success = false, Message = error.Value?.ToString() };

        public static Result Failed(string message,string reason="" ,int statusCode = 400) => new Result { Success = false,Reason = reason, Message = message, StatusCode = statusCode };
    }
}