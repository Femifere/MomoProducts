namespace MomoProducts.Server.Repositories.Remittance
{

    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Remittance;
    using MomoProducts.Server.Models.Remittance;
    using MomoProducts.Server.Dtos.RemittanceDto;

    public class CashTransferRepository : ICashTransferRepository
    {
        private readonly AppDbContext _context;

        public CashTransferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CashTransferDto> GetCashTransferByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<CashTransferDto>().FirstOrDefaultAsync(ct => ct.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<CashTransferDto>> GetAllCashTransfersAsync()
        {
            return await _context.Set<CashTransferDto>().ToListAsync();
        }

        public async Task CreateCashTransferAsync(CashTransferDto cashTransfer)
        {
            _context.Set<CashTransferDto>().Add(cashTransfer);
            await _context.SaveChangesAsync();
        }
    }

}
