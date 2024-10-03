using MomoProducts.Server.Models.Collections;


namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IRequestToWithdrawRepository
    {
        Task<RequestToWithdraw> GetRequestToWithdrawByReferenceIdAsync(string referenceId);
        Task<IEnumerable<RequestToWithdraw>> GetAllRequestsToWithdrawAsync();
        Task CreateRequestToWithdrawAsync(RequestToWithdraw requestToWithdraw);
    }
}
