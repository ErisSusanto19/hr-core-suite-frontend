using HRCoreSuite.Frontend.ViewModels.Common;
using HRCoreSuite.Frontend.ViewModels.Position;
using System.Net.Http.Json;

namespace HRCoreSuite.Frontend.Services
{
    public class PositionService : IPositionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PositionService> _logger;

        public PositionService(HttpClient httpClient, ILogger<PositionService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<PositionViewModel>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<PositionViewModel>>>("/api/position");
                return response?.Data ?? Enumerable.Empty<PositionViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all positions.");
                return Enumerable.Empty<PositionViewModel>();
            }
        }

        public async Task<PositionViewModel?> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<PositionViewModel>>($"/api/position/{id}");
                return response?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching position with ID {PositionId}.", id);
                return null;
            }
        }

        public async Task<PositionViewModel?> CreateAsync(PositionViewModel newPosition)
        {
            try
            {
                var request = new PositionRequest { Name = newPosition.Name };
                var response = await _httpClient.PostAsJsonAsync("/api/position", request);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<PositionViewModel>>();
                    return apiResponse?.Data;
                }

                _logger.LogWarning("Failed to create position. Status: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while creating a new position.");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, PositionViewModel positionToUpdate)
        {
            try
            {
                var request = new PositionRequest { Name = positionToUpdate.Name };
                var response = await _httpClient.PutAsJsonAsync($"/api/position/{id}", request);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to update position {PositionId}. Status: {StatusCode}", id, response.StatusCode);
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating position {PositionId}.", id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/position/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to delete position {PositionId}. Status: {StatusCode}", id, response.StatusCode);
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while deleting position {PositionId}.", id);
                return false;
            }
        }
    }
}