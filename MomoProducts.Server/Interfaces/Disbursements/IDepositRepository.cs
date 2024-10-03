using MomoProducts.Server.Models.Disbursements;


namespace MomoProducts.Server.Interfaces.Disbursements
{
    public interface IDepositRepository
    {

        Task<Deposit> GetDepositByExternalIdAsync(string externalId);
        Task<IEnumerable<Deposit>> GetAllDepositsAsync();
        Task CreateDepositAsync(Deposit deposit);
    }
}
