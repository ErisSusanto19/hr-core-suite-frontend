using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels
{
    public class PositionViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}