using System;

namespace PoS.Sell.Domain.Discount
{
    public class DiscountRequest : DiscuountBase
    {
        public string ProductDescription { get; set; }
    }
}
