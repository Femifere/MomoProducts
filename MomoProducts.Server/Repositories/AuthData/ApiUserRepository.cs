namespace MomoProducts.Server.Repositories.AuthData
{
    using MomoProducts.Server.Interfaces.AuthData;
    using MomoProducts.Server.Models.AuthData;
    using Microsoft.EntityFrameworkCore;

    public class ApiUserRepository : IApiUserRepository
    {
        private readonly DbContext _context;

        public ApiUserRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<ApiUser> GetApiUserAsync(string referenceId)
        {
            return await _context.Set<ApiUser>().FirstOrDefaultAsync(u => u.ReferenceId == referenceId);
        }
    }
}
