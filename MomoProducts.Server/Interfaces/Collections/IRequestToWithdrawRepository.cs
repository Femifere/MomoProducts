using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.s.Collections;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IRequestToWithdrawRepository
    {
        Task<RequestToWithdraw> GetRequestToWithdrawByReferenceIdAsync(string referenceId);
        Task<IEnumerable<RequestToWithdraw>> GetAllRequestsToWithdrawAsync();
        Task CreateRequestToWithdrawAsync(RequestToWithdraw requestToWithdraw);
    }
}
