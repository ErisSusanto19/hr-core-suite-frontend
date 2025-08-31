using System.ComponentModel.DataAnnotations;

namespace HRCoreSuite.Frontend.ViewModels.Employee
{
    public class EmployeeRequest
    {
        [Required(ErrorMessage = "Nomor Pegawai wajib diisi.")]
        [Display(Name = "Nomor Pegawai")]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Pegawai wajib diisi.")]
        [Display(Name = "Nama Pegawai")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tanggal Mulai Kontrak wajib diisi.")]
        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Mulai Kontrak")]
        public DateOnly ContractStartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required(ErrorMessage = "Tanggal Selesai Kontrak wajib diisi.")]
        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Selesai Kontrak")]
        public DateOnly ContractEndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddYears(1));

        [Required(ErrorMessage = "Cabang wajib dipilih.")]
        [Display(Name = "Cabang")]
        public Guid? BranchId { get; set; }

        [Required(ErrorMessage = "Jabatan wajib dipilih.")]
        [Display(Name = "Jabatan")]
        public Guid? PositionId { get; set; }
    }
}