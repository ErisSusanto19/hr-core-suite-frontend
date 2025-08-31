using System.ComponentModel.DataAnnotations;

namespace HRCoreSuite.Frontend.ViewModels.Branch
{
    public class BranchRequest
    {
        [Required(ErrorMessage = "Nama Cabang wajib diisi.")]
        public string Name { get; set; } = string.Empty;
    }
}