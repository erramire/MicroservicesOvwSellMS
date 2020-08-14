using PoS.Sell.Domain.AggregateModels.Discount;
using System;
using System.Collections.Generic;

namespace PoS.Sell.Domain.Discount
{
    public class DiscountResult: DiscuountBase
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public DiscountResultCode Code { get; set; }
    }
}