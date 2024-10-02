namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
   

    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
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
