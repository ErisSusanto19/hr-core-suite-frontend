using HRCoreSuite.Frontend.ViewModels.Common;
using System.Net.Http.Headers;
using HRCoreSuite.Frontend.ViewModels.Auth;
using HRCoreSuite.Frontend.ViewModels.Employee;
using HRCoreSuite.Frontend.ViewModels;

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

        public async Task<LoginResponseDto?> LoginAsync(string username, string password)
        {
            var loginRequest = new LoginRequestDto { Username = username, Password = password };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
                    if (apiResponse != null && apiResponse.Success && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                
                _logger.LogWarning("Login failed for username: {Username}. Status: {StatusCode}", username, response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred during login attempt for username: {Username}", username);
                return null;
            }
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
        
        public async Task<UploadResultViewModel> UploadEmployeesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new UploadResultViewModel { IsSuccess = false, Message = "Tidak ada file yang dipilih." };
            }

            using var content = new MultipartFormDataContent();

            using var fileStream = file.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            
            content.Add(streamContent, "file", file.FileName);

            try
            {

                var response = await _httpClient.PostAsync("/api/employee/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("File {FileName} uploaded successfully. Response: {Response}", file.FileName, responseBody);
                    return new UploadResultViewModel { FileName = file.FileName, IsSuccess = true, Message = "File berhasil diunggah dan sedang diproses." };
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to upload file. Status: {StatusCode}. Body: {Body}", response.StatusCode, errorBody);
                    return new UploadResultViewModel { FileName = file.FileName, IsSuccess = false, Message = $"Gagal mengunggah file. Server merespons dengan: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred during file upload.");
                return new UploadResultViewModel { FileName = file.FileName, IsSuccess = false, Message = "Terjadi kesalahan koneksi saat mengunggah file." };
            }
        }
    }
}