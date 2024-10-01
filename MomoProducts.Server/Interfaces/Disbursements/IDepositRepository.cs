using MomoProducts.Server.Models.Disbursements;

namespace MomoProducts.Server.Interfaces.Disbursements
{
    public interface IDepositRepository
    {
        Task<Deposit> GetDepositByReferenceIdAsync(string referenceId);
        Task<IEnumerable<Deposit>> GetAllDepositsAsync();
        Task CreateDepositAsync(Deposit deposit);
    }
}
