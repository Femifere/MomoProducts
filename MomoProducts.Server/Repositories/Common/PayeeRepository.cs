namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using MomoProducts.Server.Dtos.CommonDto;

    public class PayeeRepository : IPayeeRepository
    {
        private readonly AppDbContext _context;

        public PayeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PayeeDto> GetPayeeAsync(string partyIdType, string partyId)
        {
            return await _context.Set<PayeeDto>().FirstOrDefaultAsync(p => p.PartyIdType == partyIdType && p.PartyId == partyId);
        }
    }

}
