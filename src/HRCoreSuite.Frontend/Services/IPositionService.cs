using HRCoreSuite.Frontend.ViewModels;

namespace HRCoreSuite.Frontend.Services
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionViewModel>> GetAllAsync();
    }
}