using MomoProducts.Server.Models.Collections;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IRequestToPayRepository
    {
        Task<RequesttoPay> GetRequestToPayByExternalIdAsync(string externalId);
        Task<IEnumerable<RequesttoPay>> GetAllRequestsToPayAsync();
        Task CreateRequestToPayAsync(RequesttoPay requestToPay);
    }
}
