using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface IDiscountRepository
    {
        Task<string> Add(AggregateModels.DiscountsRO.Discount entity);
    }
}
