using MomoProducts.Server.Models.Remittance;
using MomoProducts.Server.Dtos.RemittanceDto;

namespace MomoProducts.Server.Interfaces.Remittance
{
    public interface ICashTransferRepository
    {
        Task<CashTransferDto> GetCashTransferByReferenceIdAsync(string referenceId);
        Task<IEnumerable<CashTransferDto>> GetAllCashTransfersAsync();
        Task CreateCashTransferAsync(CashTransferDto cashTransferDto);
    }
}
