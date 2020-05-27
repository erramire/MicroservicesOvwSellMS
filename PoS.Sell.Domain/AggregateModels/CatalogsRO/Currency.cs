using PoS.Sell.Domain.Contracts;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace PoS.Sell.Domain.AggregateModels.Catalogs
{
    public class Currency
    {
        private readonly ICatalogRepository _catalogRepository;
        [JsonProperty(PropertyName = "id")]
        public string Id { get=> CurrencyId.ToString();  }
        public Guid CurrencyId { get; set; }
        public string Description { get; set; }

        public Currency() { 
        }

        public Currency(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task<Guid> GetCurrencyIdAsync(string description) {
            Guid id = Guid.Empty;

            Currency currency = await _catalogRepository.GetByDescription(description);
            id = currency.CurrencyId;

            return id;
        }

        public async Task<string> CreateCurrencyAsync() {
            string result = String.Empty;
            try
            {
                result = await _catalogRepository.Add(this);
            }
            catch (Exception e)
            {

                throw e;
            }        
            return result;
        }
    }
}
