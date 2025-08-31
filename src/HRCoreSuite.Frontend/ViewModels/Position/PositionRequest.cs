using System.ComponentModel.DataAnnotations;

namespace HRCoreSuite.Frontend.ViewModels.Position
{
    public class PositionRequest
    {
        [Required(ErrorMessage = "Nama Jabatan wajib diisi.")]
        public string Name { get; set; } = string.Empty;
    }
}