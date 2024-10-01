using MomoProducts.Server.Models.Common;

namespace MomoProducts.Server.Interfaces.Common
{
    public interface IMoneyRepository
    {
        Task<Money> GetMoneyAsync();
    }
}
