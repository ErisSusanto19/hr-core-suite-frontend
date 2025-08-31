using HRCoreSuite.Frontend.ViewModels;
using HRCoreSuite.Frontend.ViewModels.Common;

namespace HRCoreSuite.Frontend.Services
{
    public class PositionService : IPositionService
    {
        private readonly HttpClient _httpClient;
        public PositionService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<IEnumerable<PositionViewModel>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<PositionViewModel>>>("/api/position");
            return response?.Data ?? Enumerable.Empty<PositionViewModel>();
        }
    }
}