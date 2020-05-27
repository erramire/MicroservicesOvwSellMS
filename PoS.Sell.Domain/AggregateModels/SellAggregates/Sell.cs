using PoS.Sell.Domain.AggregateModels.ValueObject;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Threading;

namespace PoS.Sell.Domain.AggregateModels.SellAggregates
{
    public class Sell: IAggregateRoot
    {
        public IReadOnlyCollection<SellItem> SellItems;

        public Sell() { 
        }

        public string Folio_Venta { get; set; }
        public string StoreId{ get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public Amount Amount  { get; set; }
        public Amount Cash { get; set; }
        public int ItemsQuantity { get; set; }
        public Guid CashDeskID { get; set; }
        public StatusSell Status { get; set; }





    }
}
