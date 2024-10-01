namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using MomoProducts.Server.Dtos.CommonDto;

    public class TransferRepository : ITransferRepository
    {
        private readonly AppDbContext _context;

        public TransferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TransferDto> GetTransferByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<TransferDto>().FirstOrDefaultAsync(t => t.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<TransferDto>> GetAllTransfersAsync()
        {
            return await _context.Set<TransferDto>().ToListAsync();
        }

        public async Task CreateTransferAsync(TransferDto transferDto)
        {
            _context.Set<TransferDto>().Add(transferDto);
            await _context.SaveChangesAsync();
        }
    }

}
