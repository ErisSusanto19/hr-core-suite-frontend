using HRCoreSuite.Frontend.ViewModels.Common;
using HRCoreSuite.Frontend.ViewModels.Employee;

namespace HRCoreSuite.Frontend.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllAsync();
        Task<EmployeeViewModel?> GetByIdAsync(Guid id);

        Task<ServiceResult<EmployeeViewModel>> CreateAsync(EmployeeRequest employee);
        Task<ServiceResult<bool>> UpdateAsync(Guid id, EmployeeRequest employee);
        Task<bool> DeleteAsync(Guid id);

        Task<UploadResultViewModel> UploadAsync(IFormFile file);
    }
}