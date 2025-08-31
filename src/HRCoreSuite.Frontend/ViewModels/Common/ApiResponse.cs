using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels.Common
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        [JsonPropertyName("data")]
        public T? Data { get; init; }

        [JsonPropertyName("errors")]
        public object? Errors { get; init; }
    }
}