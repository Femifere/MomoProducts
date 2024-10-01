namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using MomoProducts.Server.Dtos.CollectionsDto;

    public class RequestToWithdrawRepository : IRequestToWithdrawRepository
    {
        private readonly AppDbContext _context;

        public RequestToWithdrawRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RequestToWithdrawDto> GetRequestToWithdrawByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<RequestToWithdrawDto>().FirstOrDefaultAsync(rtw => rtw.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<RequestToWithdrawDto>> GetAllRequestsToWithdrawAsync()
        {
            return await _context.Set<RequestToWithdrawDto>().ToListAsync();
        }

        public async Task CreateRequestToWithdrawAsync(RequestToWithdrawDto requestToWithdrawDto)
        {
            _context.Set<RequestToWithdrawDto>().Add(requestToWithdrawDto);
            await _context.SaveChangesAsync();
        }
    }

}
