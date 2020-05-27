using PoS.Sell.Domain.AggregateModels.TaxRO;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using PoS.Sell.Domain.Contracts;

namespace PoS.Sell.Domain.AggregateModels.ProductRO
{
    public class Product
    {
        private readonly IProductRepository _productRepository;
        [JsonProperty(PropertyName = "id")]
        public string SKU { get; set; }
        public Amount Price { get; set; }
        public IEnumerable<Tax> Taxes { get; set; }
        public string Description { get; set; }

        public Product() { 
        }

        public Product(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductBySKUAsync(string sku) 
        {
            Product product = new Product();
            product = await _productRepository.GetById(sku);
            return product;
        }

        public async Task<string> CreateProductAsync()
        {
            string result = String.Empty;
            try
            {
                result = await _productRepository.Add(this);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }


    }
}
