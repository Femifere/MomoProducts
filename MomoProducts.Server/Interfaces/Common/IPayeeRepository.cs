
using MomoProducts.Server.Models.Common;


namespace MomoProducts.Server.Interfaces.Common
{
    public interface IPayeeRepository
    {
        Task<Payee> GetPayeeAsync(string partyIdType, string partyId);
    }
}
