
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.s.Common;

namespace MomoProducts.Server.Interfaces.Common
{
    public interface IPayeeRepository
    {
        Task<Payee> GetPayeeAsync(string partyIdType, string partyId);
    }
}
