namespace MomoProducts.Server.Repositories.Collections
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Collections;
    using MomoProducts.Server.Models.Collections;
    using MomoProducts.Server.Dtos.CollectionsDto;

    public class RequestToPayRepository : IRequestToPayRepository
    {
        private readonly AppDbContext _context;

        public RequestToPayRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RequestToPayDto> GetRequestToPayByReferenceIdAsync(string referenceId)
        {
            return await _context.Set<RequestToPayDto>().FirstOrDefaultAsync(rtp => rtp.ReferenceId == referenceId);
        }

        public async Task<IEnumerable<RequestToPayDto>> GetAllRequestsToPayAsync()
        {
            return await _context.Set<RequestToPayDto>().ToListAsync();
        }

        public async Task CreateRequestToPayAsync(RequestToPayDto requestToPayDto)
        {
            _context.Set<RequestToPayDto>().Add(requestToPayDto);
            await _context.SaveChangesAsync();
        }
    }

}
