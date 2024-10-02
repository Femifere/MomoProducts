using MomoProducts.Server.Models.Common;
using MomoProducts.Server.s.Common;
namespace MomoProducts.Server.Interfaces.Common
{
    public interface IMoneyRepository
    {
        Task<Money> GetMoneyAsync();
    }
}
