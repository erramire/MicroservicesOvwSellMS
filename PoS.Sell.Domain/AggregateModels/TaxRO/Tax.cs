using Newtonsoft.Json;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.TaxRO
{
    public class Tax
    {
        private readonly ITaxRepository _taxRepository;
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }        
        public decimal Percentage { get; set; }
        public string Description { get; set; }        
        public Tax() { }
        public Tax(ITaxRepository taxRepository) {
            _taxRepository = taxRepository;
        }
        public async Task<Tax> GetTaxByDescrition(string description) {
            Tax tax = new Tax();
            tax = await _taxRepository.GetByDescription(description);
            return tax;
        }

        public async Task<string> CreateTaxAsync()
        {
            string result = String.Empty;
            try
            {
                result = await _taxRepository.Add(this);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }
    }
}
