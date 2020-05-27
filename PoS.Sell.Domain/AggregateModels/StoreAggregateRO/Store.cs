using Newtonsoft.Json;
using PoS.Sell.Domain.AggregateModels.StoreAggregateRO;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.StoreAggregate
{
    public class Store
    {
        private IStoreRepository _storeRepository;
        [JsonProperty(PropertyName = "id")]
        public string Key { get; set; }
        public string Name { get; set; }

        public IEnumerable<CashDesk> CashDesks { get; set; }

        public Store() { }

        public Store(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }


        public async Task<string> CreateStoreAsync()
        {
            string result = String.Empty;
            try
            {
                result = await _storeRepository.Add(this);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }

        public async Task<Store> GetStoreByKeyAsync(string key)
        {
            Store store = new Store();
            store = await _storeRepository.GetById(key);
            return store;
        }
    }
}
