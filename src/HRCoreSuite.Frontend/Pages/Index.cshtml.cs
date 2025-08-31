using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.ViewModels;
using HRCoreSuite.Frontend.ViewModels.Branch;
using HRCoreSuite.Frontend.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IBranchService _branchService;
        private readonly IPositionService _positionService;

        public IEnumerable<EmployeeViewModel> Employees { get; private set; } = Enumerable.Empty<EmployeeViewModel>();
        public IEnumerable<BranchViewModel> Branches { get; private set; } = Enumerable.Empty<BranchViewModel>();
        public IEnumerable<PositionViewModel> Positions { get; private set; } = Enumerable.Empty<PositionViewModel>();

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
                var employeesTask = _employeeService.GetAllAsync();
                var branchesTask = _branchService.GetAllAsync();
                var positionsTask = _positionService.GetAllAsync();

                await Task.WhenAll(employeesTask, branchesTask, positionsTask);

                Employees = employeesTask.Result;
                Branches = branchesTask.Result;
                Positions = positionsTask.Result;

                _logger.LogInformation("Successfully loaded all data for the Index page.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while loading data for the Index page.");
                ErrorMessage = "Gagal memuat data dari server. Silakan coba lagi nanti.";
            }
        }
    }
}