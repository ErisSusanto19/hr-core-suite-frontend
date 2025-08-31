using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HRCoreSuite.Frontend.ViewModels.Position
{
    public class PositionViewModel
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Nama Jabatan wajib diisi.")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}