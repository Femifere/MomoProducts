using MomoProducts.Server.Models.Collections;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentByReferenceIdAsync(string referenceId);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task CreatePaymentAsync(Payment payment);
    }
}
