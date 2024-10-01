namespace MomoProducts.Server.Repositories.Common
{
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;
    using Microsoft.EntityFrameworkCore;

    public class MoneyRepository : IMoneyRepository
    {
        private readonly DbContext _context;

        public MoneyRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Money> GetMoneyAsync()
        {
            return await _context.Set<Money>().FirstOrDefaultAsync();
        }
    }

}
