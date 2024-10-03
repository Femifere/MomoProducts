namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    

    public class RequestToPayRepository : IRequestToPayRepository
    {
        private readonly AppDbContext _context;

        public RequestToPayRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RequesttoPay> GetRequestToPayByExternalIdAsync(string externalId)
        {
            return await _context.Set<RequesttoPay>().FirstOrDefaultAsync(rtp => rtp.ExternalId == externalId);
        }

        public async Task<IEnumerable<RequesttoPay>> GetAllRequestsToPayAsync()
        {
            return await _context.Set<RequesttoPay>().ToListAsync();
        }

        public async Task CreateRequestToPayAsync(RequesttoPay requesttoPay)
        {
            _context.Set<RequesttoPay>().Add(requesttoPay);
            await _context.SaveChangesAsync();
        }
    }

}
