using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoS.Sell.Domain.AggregateModels.TaxRO;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.TaxRO.Tests
{
    [TestClass()]
    public class TaxTests
    {
        public ITaxRepository _taxRepository { get; set; }

        [TestInitialize]
        public void Setup()
        {

            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddUserSecrets("ccd05d39-4c3d-42e9-ac50-87bb97e1d73b")
                                .Build();

            _taxRepository = new TaxRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));

        }
        [TestMethod()]
        public async Task GetTaxByDescriptionTestAsync()
        {
            bool result = true;
            bool expectedResult = true;
            Tax Tax = new Tax(_taxRepository);
            Tax taxResult = await Tax.GetTaxByDescrition("IVA");
            if (taxResult ==null)
            {
                result = false;
            }
            Assert.AreEqual(expectedResult, result);
            
        }

        [TestMethod()]
        public async Task CreateTaxAsyncTestAsync()
        {
            string expectedResult = "Item Created";
            string result = String.Empty;
            Tax Tax = new Tax(_taxRepository);
            Tax.Id = Guid.NewGuid().ToString();
            Tax.Description = "IVA";
            Tax.Percentage = 16;
            result = await Tax.CreateTaxAsync();

            Assert.AreEqual(expectedResult, result);
        }
    }
}