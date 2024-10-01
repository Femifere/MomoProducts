using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Dtos.DisbursementsDto;

namespace MomoProducts.Server.Interfaces.Disbursements
{
    public interface IDepositRepository
    {
        Task<DepositDto> GetDepositByReferenceIdAsync(string referenceId);
        Task<IEnumerable<DepositDto>> GetAllDepositsAsync();
        Task CreateDepositAsync(DepositDto depositDto);
    }
}
