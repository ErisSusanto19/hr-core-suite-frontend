using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; }

        public T? Data { get; }

        public object? Errors { get; }

        [JsonConstructor]
        public ApiResponse(bool success, T? data, object? errors)
        {
            Success = success;
            Data = data;
            Errors = errors;
        }
    }
}