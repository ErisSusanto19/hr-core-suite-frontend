using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels.Branch
{
    public class BranchViewModel
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}