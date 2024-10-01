using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Dtos.CollectionsDto;

namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IInvoiceRepository
    {
        Task<InvoiceDto> GetInvoiceByReferenceIdAsync(string referenceId);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task CreateInvoiceAsync(InvoiceDto invoiceDto);
        Task UpdateInvoiceAsync(InvoiceDto invoiceDto);
    }
}
