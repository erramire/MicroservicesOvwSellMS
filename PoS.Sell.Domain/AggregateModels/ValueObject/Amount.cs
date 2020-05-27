using PoS.Sell.Domain.AggregateModels.Catalogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.ValueObject
{
    public class Amount : PoS.Sell.Domain.Contracts.ValueObject
    {
        public Guid CurrencyId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public Amount() {  }

        public Amount(Guid currencyId, decimal amount)
        {
            CurrencyId = currencyId;
            TotalAmount = amount;
        }

        public Amount(string currency, decimal amount) {
            CurrencyId = GetCurrencyAsync(currency).Result;
            TotalAmount = amount;
        }

        /// <summary>
        /// Method to get the currency guid based in the description
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        private async Task<Guid> GetCurrencyAsync(string currency) {
            Currency currencyType = new Currency();
            Guid currencyId = await currencyType.GetCurrencyIdAsync(currency);
            return currencyId;
        }

        /// <summary>
        /// Method that returns the value object values 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CurrencyId;
            yield return TotalAmount;
        }
    }
}
