namespace MomoProducts.Server.Repositories.Common
{
    using Microsoft.EntityFrameworkCore;
    using MomoProducts.Server.Interfaces.Common;
    using MomoProducts.Server.Models.Common;


    public class MoneyRepository : IMoneyRepository
    {
        private readonly AppDbContext _context;

        public MoneyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Money> GetMoneyAsync()
        {
            return await _context.Set<Money>().FirstOrDefaultAsync();
        }
    }

}
