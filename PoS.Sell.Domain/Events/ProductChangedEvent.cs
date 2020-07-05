using PoS.Sell.Domain.AggregateModels.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.Events
{
    public class ProductChangedEvent
    {
        public string SKU { get; set; }
        public Amount Price { get; set; }        
        public string Description { get; set; }
    }
}
