namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using MomoProducts.Server.Dtos.CollectionsDto;

    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> GetPaymentByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<PaymentDto>().FirstOrDefaultAsync(p => p.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            return await _context.Set<PaymentDto>().ToListAsync();
        }

        public async Task CreatePaymentAsync(PaymentDto paymentDto)
        {
            _context.Set<PaymentDto>().Add(paymentDto);
            await _context.SaveChangesAsync();
        }
    }

}
