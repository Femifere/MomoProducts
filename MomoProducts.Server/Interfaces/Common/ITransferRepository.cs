using MomoProducts.Server.Models.Common;
using MomoProducts.Server.s.Common;
namespace MomoProducts.Server.Interfaces.Common
{
    public interface ITransferRepository
    {
        Task<Transfer> GetTransferByReferenceIdAsync(string referenceId);
        Task<IEnumerable<Transfer>> GetAllTransfersAsync();
        Task CreateTransferAsync(Transfer transfer);
    }
}
