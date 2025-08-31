using HRCoreSuite.Frontend.ViewModels.Branch;

namespace HRCoreSuite.Frontend.Services
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchViewModel>> GetAllAsync();
        Task<BranchViewModel?> GetByIdAsync(Guid id);
        Task<BranchViewModel?> CreateAsync(BranchViewModel newBranch);
        Task<bool> UpdateAsync(Guid id, BranchViewModel branchToUpdate);
        Task<bool> DeleteAsync(Guid id);
    }
}