using PoS.Sell.Domain.AggregateModels.ProductRO;
using PoS.Sell.Domain.AggregateModels.TaxRO;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PoS.Sell.Domain.AggregateModels.SellAggregates
{
    public class SellItem
    {
        /// <summary>
        /// Properties
        /// </summary>
        public Amount Price { get; set; }
        public string Description { get; set; }
        public IEnumerable<Tax> Taxes { get; set; }
        public string SKU { get; set; }
        public Guid LineSaleId { get; set; }
        public int QuantitySold { get; set; }
        public Amount TotalPaid { get; set; }
        public Amount TaxPaid { get; set; }


        /// <summary>
        /// Constructor base
        /// </summary>
        public SellItem() {
            LineSaleId = Guid.NewGuid();
            Taxes = new List<Tax>();
        }

        /// <summary>
        /// Constructor to initialize entity with the product and the quantity of products sold
        /// </summary>
        public SellItem(Product product, int quantitySold ) {
            LineSaleId = Guid.NewGuid();
            Price = product.Price;
            Description = product.Description;
            Taxes = product.Taxes;
            SKU = product.SKU;
            QuantitySold = quantitySold;
            TaxPaid = CalculateTotalTaxes();
            TotalPaid = CalculateTotalPaid(Price.CurrencyId);
        }

        public void AddQuantityItemSold() {
            QuantitySold += 1;
            TaxPaid = CalculateTotalTaxes();
            TotalPaid = CalculateTotalPaid(Price.CurrencyId);
        }

        public bool SaveSellItem() {
            bool result;
            throw new NotImplementedException();
            return result;
        }

        /// <summary>
        /// Calculates total amount to pay
        /// </summary>

        private Amount CalculateTotalPaid(Guid currencyId)
        {
            return new Amount(currencyId, (Price.TotalAmount * QuantitySold) + TaxPaid.TotalAmount);
        }

        /// <summary>
        /// Calculates total tax amount
        /// </summary>
        /// <returns></returns>
        private Amount CalculateTotalTaxes()
        {            
            decimal totaltax = 0;
            decimal price = Price.TotalAmount;
            foreach (var tax in Taxes)
            {
                totaltax += QuantitySold * price * tax.Percentage;
            }
            return new Amount(Price.CurrencyId, totaltax);
        }
    }
}
