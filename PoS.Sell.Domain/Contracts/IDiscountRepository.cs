using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.Contracts
{
    public interface IDiscountRepository
    {
        public string Add(Discount.Discount entity);       
    }
}
