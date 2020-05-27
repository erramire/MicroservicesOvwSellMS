using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoS.Sell.Domain.AggregateModels.ProductRO;
using PoS.Sell.Infrastructure.Repositories;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using PoS.Sell.Domain.AggregateModels.TaxRO;
using System.Linq;

namespace PoS.Sell.Domain.AggregateModels.ProductRO.Tests
{
    [TestClass()]
    public class ProductTests
    {
        private IProductRepository _productRepository;
        private ITaxRepository _taxRepository;

        [TestInitialize]
        public void Setup()
        {

            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddUserSecrets("ccd05d39-4c3d-42e9-ac50-87bb97e1d73b")
                                .Build();

            _productRepository = new ProductRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            _taxRepository = new TaxRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));

        }

        [TestMethod()]
        public async Task GetProductBySKUTestAsync()
        {
            bool result = true;
            bool expectedResult = true;
            Product product = new Product(_productRepository);
            Product productResult = await product.GetProductBySKUAsync("7506105606053");
            if (productResult == null)
            {
                result = false;
            }
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public async Task CreateProductAsyncTestAsync()
        {
            string expectedResult = "Item Created";
            string result = String.Empty;
            Product product = new Product(_productRepository);
            product.SKU = "7506105606053";
            product.Description = "Trident 18s";
            product.Price = new Amount(new Guid("50bca81b-4099-4087-8469-874fdc6906cf"), 14);
            Tax auxTax = new Tax(_taxRepository);
            auxTax = await auxTax.GetTaxByDescrition("IVA");
            List<Tax> taxes = new List<Tax>();
            taxes.Add(auxTax);
            product.Taxes = taxes;
            
            result = await product.CreateProductAsync();

            Assert.AreEqual(expectedResult, result);
        }
    }
}