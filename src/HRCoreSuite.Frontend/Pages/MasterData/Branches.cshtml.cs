using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.ViewModels.Branch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages.MasterData
{
    public class BranchesModel : PageModel
    {
        private readonly IBranchService _branchService;
        private readonly ILogger<BranchesModel> _logger;

        public IEnumerable<BranchViewModel> Branches { get; private set; } = new List<BranchViewModel>();

        [BindProperty]
        public BranchViewModel Branch { get; set; } = new();

        public BranchesModel(IBranchService branchService, ILogger<BranchesModel> logger)
        {
            _branchService = branchService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Branches = await _branchService.GetAllAsync();
        }

        public async Task<IActionResult> OnGetBranchForEditAsync(Guid id)
        {
            var branch = await _branchService.GetByIdAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return new JsonResult(branch);
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ShowModalOnError"] = true; 
                Branches = await _branchService.GetAllAsync();
                return Page();
            }

            if (!Branch.Id.HasValue || Branch.Id.Value == Guid.Empty)
            {
                
                await _branchService.CreateAsync(Branch);
            }
            else
            {
                await _branchService.UpdateAsync(Branch.Id.Value, Branch);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _branchService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}