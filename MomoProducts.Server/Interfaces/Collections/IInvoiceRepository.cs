using MomoProducts.Server.Models.Collections;


namespace MomoProducts.Server.Interfaces.Collections
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoiceByReferenceIdAsync(string referenceId);
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task CreateInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(Invoice invoice);
    }
}
