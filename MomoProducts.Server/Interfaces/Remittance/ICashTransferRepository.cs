using MomoProducts.Server.Models.Remittance;
using MomoProducts.Server.s.Remittance;

namespace MomoProducts.Server.Interfaces.Remittance
{
    public interface ICashTransferRepository
    {
        Task<CashTransfer> GetCashTransferByReferenceIdAsync(string referenceId);
        Task<IEnumerable<CashTransfer>> GetAllCashTransfersAsync();
        Task CreateCashTransferAsync(CashTransfer cashTransfer);
    }
}
