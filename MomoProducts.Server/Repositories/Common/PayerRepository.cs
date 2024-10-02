namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    
    public class PayerRepository : IPayerRepository
    {
        private readonly AppDbContext _context;

        public PayerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payer> GetPayerAsync(string partyIdType, string partyId)
        {
            return await _context.Set<Payer>().FirstOrDefaultAsync(p => p.PartyIdType == partyIdType && p.PartyId == partyId);
        }
    }

}
