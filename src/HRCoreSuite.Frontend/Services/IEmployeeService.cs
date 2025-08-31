using HRCoreSuite.Frontend.ViewModels.Employee;

namespace HRCoreSuite.Frontend.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllAsync();
        Task<UploadResultViewModel> UploadAsync(IFormFile file);
    }
}