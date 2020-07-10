namespace PoS.Sell.Domain.AggregateModels.DiscountsRO
{
    public class DiscountRequest : DiscountBase
    {
        public string ProductIds { get; set; }
    }
}