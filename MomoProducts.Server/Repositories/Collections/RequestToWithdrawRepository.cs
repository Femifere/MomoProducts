namespace MomoProducts.Server.Repositories.Collections
{
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using Microsoft.EntityFrameworkCore;

    public class RequestToWithdrawRepository : IRequestToWithdrawRepository
    {
        private readonly DbContext _context;

        public RequestToWithdrawRepository(DbContext context)
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
