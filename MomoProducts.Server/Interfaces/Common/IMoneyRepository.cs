using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Dtos.CommonDto;
namespace MomoProducts.Server.Interfaces.Common
{
    public interface IMoneyRepository
    {
        Task<MoneyDto> GetMoneyAsync();
    }
}
