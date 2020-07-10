using System;

namespace PoS.Sell.Domain.AggregateModels.DiscountsRO
{
    public class DiscountBase
    {
        public decimal Percentage { get; set; }
        public string Description { get; set; }        
        public DateTime ValidDate { get; set; }
    }
}
