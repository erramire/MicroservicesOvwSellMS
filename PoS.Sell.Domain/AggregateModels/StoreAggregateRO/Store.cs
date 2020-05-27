using PoS.Sell.Domain.AggregateModels.StoreAggregateRO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.AggregateModels.StoreAggregate
{
    public class Store
    {
        public string Key { get;  }
        public string Name { get; }

        public IEnumerable<CashDesk> CashDesks { get; set; }

        public Store() { }




        /// <summary>
        /// Gets the store info by Id
        /// </summary>
        /// <returns></returns>
        public Store GetStoreByKey() {
            Store store = new Store();
            throw new NotImplementedException();
            return store;
        }
    }
}
