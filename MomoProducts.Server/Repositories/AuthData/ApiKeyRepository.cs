namespace MomoProducts.Server.Repositories.AuthData
{
    using MomoProducts.Server.Interfaces.AuthData;
    using MomoProducts.Server.Models.AuthData;
    using Microsoft.EntityFrameworkCore;

    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly DbContext _context;

        public ApiKeyRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<ApiKey> GetApiKeyAsync()
        {
            return await _context.Set<ApiKey>().FirstOrDefaultAsync();
        }
    }
}