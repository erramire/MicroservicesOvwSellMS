using Newtonsoft.Json;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.SellAggregates
{
    public class StatusSell: PoS.Sell.Domain.Contracts.ValueObject
    {
        private IStatusSellRepository _statusSellRepository;

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Description { get; set; }

        public StatusSell() { }
        public StatusSell(IStatusSellRepository statusSellRepository) 
        {
            _statusSellRepository = statusSellRepository;
        }

        public StatusSell(Guid id, string description) {
            Id = id.ToString();
            Description = description;
        }




        /// <summary>
        /// Allows to set the fields of the Value Object stored in the repository by Status Sell ID
        /// </summary>
        /// <param name="description"></param>
        public async Task<StatusSell> GetStatusSellByDesc(string description)
        {
            StatusSell statusSell = new StatusSell();
            statusSell = await _statusSellRepository.GetByDesc(description);
            return statusSell;
        }

        /// <summary>
        /// Gets the values of the VO
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Description;            
        }

        public async Task<string> CreateStatusSellAsync()
        {
            Id = Guid.NewGuid().ToString();
            string result = String.Empty;
            try
            {
                result = await _statusSellRepository.Add(this);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }
    }
}
