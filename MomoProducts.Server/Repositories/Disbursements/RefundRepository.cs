namespace MomoProducts.Server.Repositories.Disbursements
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Disbursements;
    using MomoProducts.Server.Models.Disbursements;
    
    public class RefundRepository : IRefundRepository
    {
        private readonly AppDbContext _context;

        public RefundRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Refund> GetRefundByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<Refund>().FirstOrDefaultAsync(r => r.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<Refund>> GetAllRefundsAsync()
        {
            return await _context.Set<Refund>().ToListAsync();
        }

        public async Task CreateRefundAsync(Refund refund)
        {
            _context.Set<Refund>().Add(refund);
            await _context.SaveChangesAsync();
        }
    }
}
