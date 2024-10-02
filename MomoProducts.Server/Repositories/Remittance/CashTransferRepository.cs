namespace MomoProducts.Server.Repositories.Remittance
{

    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Remittance;
    using MomoProducts.Server.Models.Remittance;
    
    public class CashTransferRepository : ICashTransferRepository
    {
        private readonly AppDbContext _context;

        public CashTransferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CashTransfer> GetCashTransferByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<CashTransfer>().FirstOrDefaultAsync(ct => ct.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<CashTransfer>> GetAllCashTransfersAsync()
        {
            return await _context.Set<CashTransfer>().ToListAsync();
        }

        public async Task CreateCashTransferAsync(CashTransfer cashTransfer)
        {
            _context.Set<CashTransfer>().Add(cashTransfer);
            await _context.SaveChangesAsync();
        }
    }

}
