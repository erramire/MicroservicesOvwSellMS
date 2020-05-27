using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.AggregateModels.StoreAggregateRO
{
    public class CashDesk
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public CashDesk() { }

        public CashDesk GetCashDeskById(string id) {
            CashDesk cashDesk = new CashDesk();
            throw new NotImplementedException();
            return cashDesk;
        }
    }
}
