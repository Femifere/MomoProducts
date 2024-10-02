namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
 

    public class RequestToWithdrawRepository : IRequestToWithdrawRepository
    {
        private readonly AppDbContext _context;

        public RequestToWithdrawRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RequestToWithdraw> GetRequestToWithdrawByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<RequestToWithdraw>().FirstOrDefaultAsync(rtw => rtw.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<RequestToWithdraw>> GetAllRequestsToWithdrawAsync()
        {
            return await _context.Set<RequestToWithdraw>().ToListAsync();
        }

        public async Task CreateRequestToWithdrawAsync(RequestToWithdraw requestToWithdraw)
        {
            _context.Set<RequestToWithdraw>().Add(requestToWithdraw);
            await _context.SaveChangesAsync();
        }
    }

}
