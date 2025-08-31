using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels.Auth
{
    public class LoginResponseDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}