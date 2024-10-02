using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.s.Collections;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IPreApprovalRepository
    {
        Task<PreApproval> GetPreApprovalByReferenceIdAsync(string referenceId);
        Task<IEnumerable<PreApproval>> GetAllPreApprovalsAsync();
        Task CreatePreApprovalAsync(PreApproval preApproval);
        Task UpdatePreApprovalAsync(PreApproval preApproval);
    }
}
