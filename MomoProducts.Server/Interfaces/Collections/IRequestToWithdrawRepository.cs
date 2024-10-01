using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Dtos.CollectionsDto;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IRequestToWithdrawRepository
    {
        Task<RequestToWithdrawDto> GetRequestToWithdrawByReferenceIdAsync(string referenceId);
        Task<IEnumerable<RequestToWithdrawDto>> GetAllRequestsToWithdrawAsync();
        Task CreateRequestToWithdrawAsync(RequestToWithdrawDto requestToWithdrawDto);
    }
}
