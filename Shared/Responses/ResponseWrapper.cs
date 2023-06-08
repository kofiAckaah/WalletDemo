using Shared.Enums;

namespace Shared.Responses
{
    public class ResponseWrapper<T>
    {
        public string Message { get; set; } = "Failed";
        public ResponseStatus Status { get; set; } = ResponseStatus.Failed;
        public T Data { get; set; } = default;
    }

    public static class WrapResponse
    {
        public static ResponseWrapper<T> Fail<T>(string message, T data) where T : class =>
            new ResponseWrapper<T> { Data = data, Message = message };

        public static ResponseWrapper<T> Fail<T>(string message) =>
            new ResponseWrapper<T> { Message = message };

        public static ResponseWrapper<T> Success<T>(string message, T data) where T : class =>
            new ResponseWrapper<T> { Data = data, Message = message, Status = ResponseStatus.Success };

        public static ResponseWrapper<T> Success<T>(string message) =>
            new ResponseWrapper<T> { Message = message, Status = ResponseStatus.Success };
    }
}
