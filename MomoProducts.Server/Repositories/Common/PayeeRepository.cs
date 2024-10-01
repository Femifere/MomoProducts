namespace MomoProducts.Server.Repositories.Common
{
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using Microsoft.EntityFrameworkCore;

    public class PayeeRepository : IPayeeRepository
    {
        private readonly DbContext _context;

        public PayeeRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Payee> GetPayeeAsync(string partyIdType, string partyId)
        {
            return await _context.Set<Payee>().FirstOrDefaultAsync(p => p.PartyIdType == partyIdType && p.PartyId == partyId);
        }
    }

}
