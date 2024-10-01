using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Dtos.CollectionsDto;
namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IRequestToPayRepository
    {
        Task<RequestToPayDto> GetRequestToPayByReferenceIdAsync(string referenceId);
        Task<IEnumerable<RequestToPayDto>> GetAllRequestsToPayAsync();
        Task CreateRequestToPayAsync(RequestToPayDto requestToPayDto);
    }
}
