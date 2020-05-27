using PoS.Sell.Domain.AggregateModels.TaxRO;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.AggregateModels.ProductRO
{
    public class Product
    {
        public string SKU { get; set; }
        public Amount Price { get; set; }
        public IEnumerable<Tax> Taxes { get; set; }
        public string Description { get; set; }

        public Product GetProductBySKU(string sku) 
        {
            Product product = new Product();
            throw new NotImplementedException();
            return product;
        }


    }
}
