using HRCoreSuite.Frontend.ViewModels;
using HRCoreSuite.Frontend.ViewModels.Common;

namespace HRCoreSuite.Frontend.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<EmployeeViewModel>>>("/api/employee");

                if (response != null && response.Success)
                {
                    _logger.LogInformation("Successfully fetched {Count} employee records.", response.Data?.Data.Count() ?? 0);
                    return response.Data?.Data ?? Enumerable.Empty<EmployeeViewModel>();
                }

                _logger.LogWarning("API call to /api/employee was not successful. Errors: {Errors}", response?.Errors);
                return Enumerable.Empty<EmployeeViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while fetching employees.");
                return Enumerable.Empty<EmployeeViewModel>();
            }
        }

        public async Task<IEnumerable<BranchViewModel>> GetBranchesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<BranchViewModel>>>("/api/branch");

                if (response != null && response.Success)
                {
                    _logger.LogInformation("Successfully fetched {Count} branch records.", response.Data?.Count() ?? 0);
                    return response.Data ?? Enumerable.Empty<BranchViewModel>();
                }
                
                _logger.LogWarning("API call to /api/branch was not successful. Errors: {Errors}", response?.Errors);
                return Enumerable.Empty<BranchViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while fetching branches.");
                return Enumerable.Empty<BranchViewModel>();
            }
        }

        public async Task<IEnumerable<PositionViewModel>> GetPositionsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<PositionViewModel>>>("/api/position");

                if (response != null && response.Success)
                {
                    _logger.LogInformation("Successfully fetched {Count} position records.", response.Data?.Count() ?? 0);
                    return response.Data ?? Enumerable.Empty<PositionViewModel>();
                }

                _logger.LogWarning("API call to /api/position was not successful. Errors: {Errors}", response?.Errors);
                return Enumerable.Empty<PositionViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while fetching positions.");
                return Enumerable.Empty<PositionViewModel>();
            }
        }
    }
}