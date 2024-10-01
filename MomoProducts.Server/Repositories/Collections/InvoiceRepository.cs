namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using MomoProducts.Server.Dtos.CollectionsDto;
    using System.Threading.Tasks;

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDto> GetInvoiceByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<InvoiceDto>().FirstOrDefaultAsync(i => i.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            return await _context.Set<InvoiceDto>().ToListAsync();
        }

        public async Task CreateInvoiceAsync(InvoiceDto invoiceDto)
        {
            _context.Set<InvoiceDto>().Add(invoiceDto);
            await _context.SaveChangesAsync();
        }

        // Implementation of UpdateInvoiceAsync
        public async Task UpdateInvoiceAsync(InvoiceDto invoiceDto)
        {
            // Find the existing invoice by reference ID
            var existingInvoice = await _context.Set<InvoiceDto>().FirstOrDefaultAsync(i => i.ReferenceId == invoiceDto.ReferenceId);

            if (existingInvoice != null)
            {
                // Update the fields of the existing invoice with the new data
                existingInvoice.ExternalId = invoiceDto.ExternalId;
                existingInvoice.Amount = invoiceDto.Amount;
                existingInvoice.Currency = invoiceDto.Currency;
                existingInvoice.Status = invoiceDto.Status;
                existingInvoice.ValidityDuration = invoiceDto.ValidityDuration;
                existingInvoice.IntendedPayer = invoiceDto.IntendedPayer;
                existingInvoice.Payee = invoiceDto.Payee;
                existingInvoice.Description = invoiceDto.Description;

                // Save changes to the database
                _context.Entry(existingInvoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
