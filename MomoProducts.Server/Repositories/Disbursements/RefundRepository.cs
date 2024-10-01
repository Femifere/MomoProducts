namespace MomoProducts.Server.Repositories.Disbursements
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Disbursements;
    using MomoProducts.Server.Models.Disbursements;
    using MomoProducts.Server.Dtos.DisbursementsDto;

    public class RefundRepository : IRefundRepository
    {
        private readonly AppDbContext _context;

        public RefundRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefundDto> GetRefundByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<RefundDto>().FirstOrDefaultAsync(r => r.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<RefundDto>> GetAllRefundsAsync()
        {
            return await _context.Set<RefundDto>().ToListAsync();
        }

        public async Task CreateRefundAsync(RefundDto refundDto)
        {
            _context.Set<RefundDto>().Add(refundDto);
            await _context.SaveChangesAsync();
        }
    }
}
