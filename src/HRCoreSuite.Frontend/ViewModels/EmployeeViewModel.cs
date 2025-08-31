using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels
{
    public class EmployeeViewModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("employeeNumber")]
        public string EmployeeNumber { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("contractStartDate")]
        public DateOnly ContractStartDate { get; set; }

        [JsonPropertyName("contractEndDate")]
        public DateOnly ContractEndDate { get; set; }

        [JsonPropertyName("branchName")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("positionName")]
        public string PositionName { get; set; } = string.Empty;
    }
}