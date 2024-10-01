namespace MomoProducts.Server.Repositories.Collections
{
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using Microsoft.EntityFrameworkCore;

    public class PaymentRepository : IPaymentRepository
    {
        private readonly DbContext _context;

        public PaymentRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetPaymentByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<Payment>().FirstOrDefaultAsync(p => p.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Set<Payment>().ToListAsync();
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            _context.Set<Payment>().Add(payment);
            await _context.SaveChangesAsync();
        }
    }

}
