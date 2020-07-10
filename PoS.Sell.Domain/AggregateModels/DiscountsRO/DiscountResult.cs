using System.Collections.Generic;

namespace PoS.Sell.Domain.AggregateModels.DiscountsRO
{
    public class DiscountResult : DiscountBase
    {
        public string Id { get; set; }
        public string ProductIds { get; set; }
        public DiscountResultCode Code { get; set; }
    }
}