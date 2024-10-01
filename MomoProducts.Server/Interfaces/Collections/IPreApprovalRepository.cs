using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Dtos.CollectionsDto;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IPreApprovalRepository
    {
        Task<PreApprovalDto> GetPreApprovalByReferenceIdAsync(string referenceId);
        Task<IEnumerable<PreApprovalDto>> GetAllPreApprovalsAsync();
        Task CreatePreApprovalAsync(PreApprovalDto preApprovalDto);
        Task UpdatePreApprovalAsync(PreApprovalDto preApprovalDto);
    }
}
