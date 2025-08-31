using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApiClient _apiClient;

        public IEnumerable<EmployeeViewModel> Employees { get; private set; } = Enumerable.Empty<EmployeeViewModel>();
        public IEnumerable<BranchViewModel> Branches { get; private set; } = Enumerable.Empty<BranchViewModel>();
        public IEnumerable<PositionViewModel> Positions { get; private set; } = Enumerable.Empty<PositionViewModel>();

        public string? ErrorMessage { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, ApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }
        public async Task OnGetAsync()
        {
            _logger.LogInformation("Index page loading. Fetching data from API.");

            try
            {
                var employeesTask = _apiClient.GetEmployeesAsync();
                var branchesTask = _apiClient.GetBranchesAsync();
                var positionsTask = _apiClient.GetPositionsAsync();

                await Task.WhenAll(employeesTask, branchesTask, positionsTask);

                Employees = employeesTask.Result;
                Branches = branchesTask.Result;
                Positions = positionsTask.Result;

                _logger.LogInformation("Successfully loaded all data for the Index page.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while loading data for the Index page.");
                ErrorMessage = "Failed to load data from the server. Please ensure the backend API is running and accessible.";
            }
        }
    }
}