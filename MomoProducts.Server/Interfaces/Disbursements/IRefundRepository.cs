using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Dtos.DisbursementsDto;

namespace MomoProducts.Server.Interfaces.Disbursements
{
    public interface IRefundRepository
    {
        Task<RefundDto> GetRefundByReferenceIdAsync(string referenceId);
        Task<IEnumerable<RefundDto>> GetAllRefundsAsync();
        Task CreateRefundAsync(RefundDto  refundDto);
    }
}
