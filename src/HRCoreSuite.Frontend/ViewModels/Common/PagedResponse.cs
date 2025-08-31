using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels.Common
{
    public class PagedResponse<T>
    {
        [JsonPropertyName("page")]
        public int Page { get; init; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; init; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; init; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; init; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; init; } = Enumerable.Empty<T>();
    }
}