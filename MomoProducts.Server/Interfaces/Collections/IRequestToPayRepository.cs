using MomoProducts.Server.Models.Collections;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IRequestToPayRepository
    {
        Task<RequesttoPay> GetRequestToPayByReferenceIdAsync(string referenceId);
        Task<IEnumerable<RequesttoPay>> GetAllRequestsToPayAsync();
        Task CreateRequestToPayAsync(RequesttoPay requestToPay);
    }
}
