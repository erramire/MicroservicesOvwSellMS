using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PoS.Sell.Domain.AggregateModels.StoreAggregateRO
{
    public class CashDesk
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public CashDesk() { }

        public CashDesk(string id, string description) 
        {
            Id = id;
            Description = description;
        }


    }
}
