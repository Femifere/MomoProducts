
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Dtos.CommonDto;

namespace MomoProducts.Server.Interfaces.Common
{
    public interface IPayeeRepository
    {
        Task<PayeeDto> GetPayeeAsync(string partyIdType, string partyId);
    }
}
