namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Models.Common;
    using MomoProducts.Server.Interfaces.Common;

    public class TransferRepository : ITransferRepository
    {
        private readonly DbContext _context;

        public TransferRepository(DbContext context)
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
