using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Dtos.CollectionsDto;
namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IPaymentRepository
    {
        Task<PaymentDto> GetPaymentByReferenceIdAsync(string referenceId);
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task CreatePaymentAsync(PaymentDto paymentDto);
    }
}
