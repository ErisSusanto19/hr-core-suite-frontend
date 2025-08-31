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

        public async Task<EmployeeViewModel?> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<EmployeeViewModel>>($"/api/employee/{id}");
                return response?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee with ID {EmployeeId}.", id);
                return null;
            }
        }

        public async Task<ServiceResult<EmployeeViewModel>> CreateAsync(EmployeeRequest employee)
        {
            var result = new ServiceResult<EmployeeViewModel>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/employee", employee);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeViewModel>>();
                    result.Data = apiResponse?.Data;
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    result.ErrorMessages.Add(errorResponse?.Errors?.ToString() ?? "Terjadi error yang tidak diketahui.");

                    _logger.LogWarning("Failed to create employee. Status: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessages.Add("Exception while creating a new employee.");
                _logger.LogError(ex, "Exception while creating a new employee.");
            }

            return result;
        }

        public async Task<ServiceResult<bool>> UpdateAsync(Guid id, EmployeeRequest employee)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/employee/{id}", employee);

                if (response.IsSuccessStatusCode)
                {
                    result.Data = true;
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    var errorMessage = errorResponse?.Errors?.ToString() ?? "Terjadi error yang tidak diketahui saat memperbarui data.";
  
                    result.ErrorMessages.Add(errorMessage);

                    _logger.LogWarning("Failed to update employee {EmployeeId}. Status: {StatusCode}. Reason: {Reason}", 
                        id, response.StatusCode, errorMessage);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessages.Add("Terjadi kesalahan koneksi saat memperbarui data.");
                _logger.LogError(ex, "Exception while updating employee {EmployeeId}.", id);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/employee/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to delete employee {EmployeeId}. Status: {StatusCode}", id, response.StatusCode);
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while deleting employee {EmployeeId}.", id);
                return false;
            }
        }

    }
}