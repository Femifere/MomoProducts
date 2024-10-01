using MomoProducts.Server.Models.Disbursements;

namespace MomoProducts.Server.Interfaces.Disbursements
{
    public interface IRefundRepository
    {
        Task<Refund> GetRefundByReferenceIdAsync(string referenceId);
        Task<IEnumerable<Refund>> GetAllRefundsAsync();
        Task CreateRefundAsync(Refund refund);
    }
}
