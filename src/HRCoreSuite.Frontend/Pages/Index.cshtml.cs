using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.Services.QueryParameters;
using HRCoreSuite.Frontend.ViewModels.Branch;
using HRCoreSuite.Frontend.ViewModels.Common;
using HRCoreSuite.Frontend.ViewModels.Employee;
using HRCoreSuite.Frontend.ViewModels.Position;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IBranchService _branchService;
        private readonly IPositionService _positionService;

        [BindProperty(SupportsGet = true)]
        public EmployeeQueryParameters QueryParams { get; set; } = new();
        public PagedResponse<EmployeeViewModel> Employees { get; set; } = new();
        public IEnumerable<EmployeeViewModel> ExpiringContracts { get; set; } = new List<EmployeeViewModel>();
        public IEnumerable<BranchViewModel> Branches { get; set; } = new List<BranchViewModel>();
        public IEnumerable<PositionViewModel> Positions { get; set; } = new List<PositionViewModel>();

        public string? ErrorMessage { get; private set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IEmployeeService employeeService,
            IBranchService branchService,
            IPositionService positionService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _branchService = branchService;
            _positionService = positionService;
        }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("Index page loading. Fetching data from API.");

            try
            {
                var employeesTask = _employeeService.GetAllAsync(QueryParams);
                var branchesTask = _branchService.GetAllAsync();
                var positionsTask = _positionService.GetAllAsync();
                var expiringContractsTask = _employeeService.GetExpiringContractsAsync();

                await Task.WhenAll(employeesTask, branchesTask, positionsTask, expiringContractsTask);

                Employees = employeesTask.Result;
                Branches = branchesTask.Result;
                Positions = positionsTask.Result;
                ExpiringContracts = expiringContractsTask.Result;

                _logger.LogInformation("Successfully loaded all data for the Index page.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while loading data for the Index page.");
                ErrorMessage = "Gagal memuat data dari server. Silakan coba lagi nanti.";
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            bool isSuccess = await _employeeService.DeleteAsync(id);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Data pegawai berhasil dihapus.";
            }
            else
            {
                TempData["ErrorMessage"] = "Gagal menghapus data pegawai. Mungkin data sedang digunakan atau terjadi masalah koneksi.";
            }

            return RedirectToPage();
        }
    }
}