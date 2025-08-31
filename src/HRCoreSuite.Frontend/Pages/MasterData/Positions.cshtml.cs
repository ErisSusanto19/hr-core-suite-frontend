using HRCoreSuite.Frontend.Services;
using HRCoreSuite.Frontend.ViewModels.Position;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRCoreSuite.Frontend.Pages.MasterData
{
    public class PositionsModel : PageModel
    {
        private readonly IPositionService _positionService;
        private readonly ILogger<PositionsModel> _logger;

        public IEnumerable<PositionViewModel> Positions { get; private set; } = new List<PositionViewModel>();

        [BindProperty]
        public PositionViewModel Position { get; set; } = new();

        public PositionsModel(IPositionService positionService, ILogger<PositionsModel> logger)
        {
            _positionService = positionService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Positions = await _positionService.GetAllAsync();
        }

        public async Task<IActionResult> OnGetPositionForEditAsync(Guid id)
        {
            var position = await _positionService.GetByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return new JsonResult(position);
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ShowModalOnError"] = true; 
                Positions = await _positionService.GetAllAsync();
                return Page();
            }

            if (!Position.Id.HasValue || Position.Id.Value == Guid.Empty)
            {
                await _positionService.CreateAsync(Position);
            }
            else
            {
                await _positionService.UpdateAsync(Position.Id.Value, Position);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _positionService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}