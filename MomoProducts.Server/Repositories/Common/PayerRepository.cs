namespace MomoProducts.Server.Repositories.Common
{
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using Microsoft.EntityFrameworkCore;

    public class PayerRepository : IPayerRepository
    {
        private readonly DbContext _context;

        public PayerRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Payer> GetPayerAsync(string partyIdType, string partyId)
        {
            return await _context.Set<Payer>().FirstOrDefaultAsync(p => p.PartyIdType == partyIdType && p.PartyId == partyId);
        }
    }

}
