using HRCoreSuite.Frontend.ViewModels;
using HRCoreSuite.Frontend.ViewModels.Common;
using HRCoreSuite.Frontend.ViewModels.Employee;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HRCoreSuite.Frontend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(HttpClient httpClient, ILogger<EmployeeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResponse<EmployeeViewModel>>>("/api/employee");
                return response?.Data?.Data ?? Enumerable.Empty<EmployeeViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all employees.");
                return Enumerable.Empty<EmployeeViewModel>();
            }
        }

        public async Task<UploadResultViewModel> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new UploadResultViewModel { IsSuccess = false, Message = "Tidak ada file yang dipilih." };
            }

            try
            {
                using var content = new MultipartFormDataContent();
                using var fileStream = file.OpenReadStream();
                var streamContent = new StreamContent(fileStream);
                content.Add(streamContent, "file", file.FileName);

                var response = await _httpClient.PostAsync("/api/employee/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("File {FileName} uploaded successfully.", file.FileName);
                    return new UploadResultViewModel { FileName = file.FileName, IsSuccess = true, Message = "File berhasil diunggah." };
                }
                else
                {
                    _logger.LogWarning("Failed to upload file {FileName}. Status: {StatusCode}", file.FileName, response.StatusCode);
                    return new UploadResultViewModel { FileName = file.FileName, IsSuccess = false, Message = $"Gagal mengunggah file. Status: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while uploading file {FileName}.", file.FileName);
                return new UploadResultViewModel { FileName = file.FileName, IsSuccess = false, Message = "Terjadi kesalahan koneksi saat mengunggah file." };
            }
        }
    }
}