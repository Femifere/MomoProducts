using MomoProducts.Server.Models.Common;

namespace MomoProducts.Server.Interfaces.Common
{
    public interface ITransferRepository
    {
        Task<Transfer> GetTransferByExternalIdAsync(string externalId);
        Task<IEnumerable<Transfer>> GetAllTransfersAsync();
        Task CreateTransferAsync(Transfer transfer);
    }
}
