namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using System.Threading.Tasks;

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> GetInvoiceByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<Invoice>().FirstOrDefaultAsync(i => i.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _context.Set<Invoice>().ToListAsync();
        }

        public async Task CreateInvoiceAsync(Invoice invoice)
        {
            _context.Set<Invoice>().Add(invoice);
            await _context.SaveChangesAsync();
        }

        // Implementation of UpdateInvoiceAsync
        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            // Find the existing invoice by reference ID
            var existingInvoice = await _context.Set<Invoice>().FirstOrDefaultAsync(i => i.ReferenceId == invoice.ReferenceId);

            if (existingInvoice != null)
            {
                // Update the fields of the existing invoice with the new data
                existingInvoice.ExternalId = invoice.ExternalId;
                existingInvoice.Amount = invoice.Amount;
                existingInvoice.Currency = invoice.Currency;
                existingInvoice.Status = invoice.Status;
                existingInvoice.ValidityDuration = invoice.ValidityDuration;
                existingInvoice.IntendedPayer = invoice.IntendedPayer;
                existingInvoice.Payee = invoice.Payee;
                existingInvoice.Description = invoice.Description;

                // Save changes to the database
                _context.Entry(existingInvoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
