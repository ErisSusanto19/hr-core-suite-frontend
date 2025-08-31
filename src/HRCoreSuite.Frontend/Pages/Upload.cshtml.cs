using HRCoreSuite.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages
{
    public class UploadModel : PageModel
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<UploadModel> _logger;
 
        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        [TempData]
        public string? TempDataMessage { get; set; }

        public List<string> UploadResults { get; set; } = new List<string>();

        public UploadModel(ApiClient apiClient, ILogger<UploadModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(TempDataMessage))
            {
                UploadResults.Add(TempDataMessage);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFile", "Silakan pilih file untuk diunggah.");
                return Page();
            }

            _logger.LogInformation("Attempting to upload file: {FileName}", UploadedFile.FileName);

            var result = await _apiClient.UploadEmployeesAsync(UploadedFile);

            TempDataMessage = $"File: '{result.FileName}' - Status: {(result.IsSuccess ? "Sukses" : "Gagal")} - Pesan: {result.Message}";
            _logger.LogInformation("Upload result: {Result}", TempDataMessage);

            return RedirectToPage();
        }
    }
}