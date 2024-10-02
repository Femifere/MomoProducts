namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    
    public class TransferRepository : ITransferRepository
    {
        private readonly AppDbContext _context;

        public TransferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transfer> GetTransferByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<Transfer>().FirstOrDefaultAsync(t => t.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<Transfer>> GetAllTransfersAsync()
        {
            return await _context.Set<Transfer>().ToListAsync();
        }

        public async Task CreateTransferAsync(Transfer transfer)
        {
            _context.Set<Transfer>().Add(transfer);
            await _context.SaveChangesAsync();
        }
    }

}
