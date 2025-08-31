using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.ViewModels.Branch;
using HRCoreSuite.Frontend.ViewModels.Employee;
using HRCoreSuite.Frontend.ViewModels.Position;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRCoreSuite.Frontend.Pages.Employees
{
    public class ManageModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IBranchService _branchService;
        private readonly IPositionService _positionService;

        [BindProperty]
        public EmployeeRequest Employee { get; set; } = new();

        public SelectList BranchOptions { get; set; } = new(new List<BranchViewModel>());
        public SelectList PositionOptions { get; set; } = new(new List<PositionViewModel>());

        public bool IsEditMode => Id.HasValue;
        public Guid? Id { get; set; }

        public ManageModel(IEmployeeService employeeService, IBranchService branchService, IPositionService positionService)
        {
            _employeeService = employeeService;
            _branchService = branchService;
            _positionService = positionService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            Id = id;

            if (IsEditMode)
            {
                var existingEmployee = await _employeeService.GetByIdAsync(id!.Value);
                if (existingEmployee == null)
                {
                    return RedirectToPage("/Index");
                }

                await LoadDropdownOptions(existingEmployee.BranchId, existingEmployee.PositionId);

                Employee = new EmployeeRequest
                {
                    EmployeeNumber = existingEmployee.EmployeeNumber,
                    Name = existingEmployee.Name,
                    ContractStartDate = existingEmployee.ContractStartDate,
                    ContractEndDate = existingEmployee.ContractEndDate,
                    BranchId = existingEmployee.BranchId,
                    PositionId = existingEmployee.PositionId
                };
            }
            else
            {
                await LoadDropdownOptions();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            Id = id;
            if (!ModelState.IsValid)
            {
                await LoadDropdownOptions(Employee.BranchId, Employee.PositionId);
                return Page();
            }

            if (IsEditMode)
            {
                await _employeeService.UpdateAsync(id!.Value, Employee);
            }
            else
            {
                var result = await _employeeService.CreateAsync(Employee);
                if (!result.IsSuccess)
                {
                    foreach (var error in result.ErrorMessages)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }

                    await LoadDropdownOptions(Employee.BranchId, Employee.PositionId);
                    return Page();
                }
            }

            return RedirectToPage("/Index");
        }

        private async Task LoadDropdownOptions(Guid? selectedBranchId = null, Guid? selectedPositionId = null)
        {
            var branches = await _branchService.GetAllAsync();
            var positions = await _positionService.GetAllAsync();

            BranchOptions = new SelectList(branches, "Id", "Name", selectedBranchId);
            PositionOptions = new SelectList(positions, "Id", "Name", selectedPositionId);
        }
    }
}