using HRCoreSuite.Frontend.ViewModels.Position;

namespace HRCoreSuite.Frontend.Services
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionViewModel>> GetAllAsync();
        Task<PositionViewModel?> GetByIdAsync(Guid id);
        Task<PositionViewModel?> CreateAsync(PositionViewModel newPosition);
        Task<bool> UpdateAsync(Guid id, PositionViewModel positionToUpdate);
        Task<bool> DeleteAsync(Guid id);
    }
}