using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.AggregateModels.SellAggregates
{
    public class StatusSell: PoS.Sell.Domain.Contracts.ValueObject
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public StatusSell() { }

        public StatusSell(Guid id, string description) {
            Id = id;
            Description = description;
        }

        /// <summary>
        /// Allows to set the fields of the Value Object stored in the repository by Status Sell ID
        /// </summary>
        /// <param name="Id"></param>
        public void SetStatusSellById(Guid Id) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows to set the fields of the Value Object stored in the repository by Status Sell ID
        /// </summary>
        /// <param name="description"></param>
        public void SetStatusSellByDesc(string description)
        {
            throw new NotImplementedException();
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
    }
}
