namespace MomoProducts.Server.Repositories.Disbursements
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Disbursements;
    using MomoProducts.Server.Models.Disbursements;
    
    public class DepositRepository : IDepositRepository
    {
        private readonly AppDbContext _context;

        public DepositRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Deposit> GetDepositByExternalIdAsync(string externalId)
        {
            return await _context.Set<Deposit>().FirstOrDefaultAsync(d => d.ExternalId == externalId);
        }

        public async Task<IEnumerable<Deposit>> GetAllDepositsAsync()
        {
            return await _context.Set<Deposit>().ToListAsync();
        }

        public async Task CreateDepositAsync(Deposit deposit)
        {
            _context.Set<Deposit>().Add(deposit);
            await _context.SaveChangesAsync();
        }
    }

}
