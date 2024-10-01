using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Dtos.CommonDto;
namespace MomoProducts.Server.Interfaces.Common
{
    public interface IPayerRepository
    {
        Task<PayerDto> GetPayerAsync(string partyIdType, string partyId);
    }
}
