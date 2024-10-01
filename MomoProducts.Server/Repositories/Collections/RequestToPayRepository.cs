namespace MomoProducts.Server.Repositories.Collections
{
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using Microsoft.EntityFrameworkCore;

    public class RequestToPayRepository : IRequestToPayRepository
    {
        private readonly DbContext _context;

        public RequestToPayRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<RequesttoPay> GetRequestToPayByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<RequesttoPay>().FirstOrDefaultAsync(rtp => rtp.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<RequesttoPay>> GetAllRequestsToPayAsync()
        {
            return await _context.Set<RequesttoPay>().ToListAsync();
        }

        public async Task CreateRequestToPayAsync(RequesttoPay requestToPay)
        {
            _context.Set<RequesttoPay>().Add(requestToPay);
            await _context.SaveChangesAsync();
        }
    }

}
