using HRCoreSuite.Frontend.ViewModels.Branch;
using HRCoreSuite.Frontend.ViewModels.Common;
using System.Net.Http.Json;

namespace HRCoreSuite.Frontend.Services
{
    public class BranchService : IBranchService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BranchService> _logger;

        public BranchService(HttpClient httpClient, ILogger<BranchService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<BranchViewModel>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<BranchViewModel>>>("/api/branch");
                return response?.Data ?? Enumerable.Empty<BranchViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all branches.");
                return Enumerable.Empty<BranchViewModel>();
            }
        }

        public async Task<BranchViewModel?> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<BranchViewModel>>($"/api/branch/{id}");
                return response?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching branch with ID {BranchId}.", id);
                return null;
            }
        }

        public async Task<BranchViewModel?> CreateAsync(BranchViewModel newBranch)
        {
            try
            {
                var request = new BranchRequest { Name = newBranch.Name };
                var response = await _httpClient.PostAsJsonAsync("/api/branch", request);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BranchViewModel>>();
                    return apiResponse?.Data;
                }

                _logger.LogWarning("Failed to create branch. Status: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while creating a new branch.");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, BranchViewModel branchToUpdate)
        {
            try
            {
                var request = new BranchRequest { Name = branchToUpdate.Name };
                var response = await _httpClient.PutAsJsonAsync($"/api/branch/{id}", request);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to update branch {BranchId}. Status: {StatusCode}", id, response.StatusCode);
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating branch {BranchId}.", id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/branch/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to delete branch {BranchId}. Status: {StatusCode}", id, response.StatusCode);
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while deleting branch {BranchId}.", id);
                return false;
            }
        }
    }
}