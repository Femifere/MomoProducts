namespace MomoProducts.Server.Repositories.Disbursements
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Disbursements;
    using MomoProducts.Server.Models.Disbursements;
    using MomoProducts.Server.Dtos.DisbursementsDto;

    public class DepositRepository : IDepositRepository
    {
        private readonly AppDbContext _context;

        public DepositRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DepositDto> GetDepositByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<DepositDto>().FirstOrDefaultAsync(d => d.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<DepositDto>> GetAllDepositsAsync()
        {
            return await _context.Set<DepositDto>().ToListAsync();
        }

        public async Task CreateDepositAsync(DepositDto depositDto)
        {
            _context.Set<DepositDto>().Add(depositDto);
            await _context.SaveChangesAsync();
        }
    }

}
