using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.s.Disbursements;

namespace MomoProducts.Server.Interfaces.Disbursements
{
    public interface IDepositRepository
    {
        Task<Deposit> GetDepositByReferenceIdAsync(string referenceId);
        Task<IEnumerable<Deposit>> GetAllDepositsAsync();
        Task CreateDepositAsync(Deposit deposit);
    }
}
