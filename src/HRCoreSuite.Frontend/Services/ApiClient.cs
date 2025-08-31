namespace HRCoreSuite.Frontend.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Kita akan menambahkan method untuk get employee, branch, dll di sini nanti
        // Contoh:
        // public async Task<List<EmployeeViewModel>> GetEmployeesAsync()
        // {
        //     return await _httpClient.GetFromJsonAsync<List<EmployeeViewModel>>("/api/employee");
        // }
    }
}