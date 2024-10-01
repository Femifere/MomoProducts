namespace MomoProducts.Server.Repositories.Disbursements
{
    using MomoProducts.Server.Models.Disbursements;
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Disbursements;

    public class DepositRepository : IDepositRepository
    {
        private readonly DbContext _context;

        public DepositRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Deposit> GetDepositByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<Deposit>().FirstOrDefaultAsync(d => d.ReferenceId == referenceId);
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
