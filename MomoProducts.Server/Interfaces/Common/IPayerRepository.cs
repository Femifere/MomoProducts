using MomoProducts.Server.Models.Common;

namespace MomoProducts.Server.Interfaces.Common
{
    public interface IPayerRepository
    {
        Task<Payer> GetPayerAsync(string partyIdType, string partyId);
    }
}
