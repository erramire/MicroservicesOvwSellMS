using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoS.Sell.Domain.AggregateModels.Catalogs;
using PoS.Sell.Domain.Contracts;

using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.Catalogs.Tests
{


    [TestClass()]
    public class CurrencyTests
    {
        public ICatalogRepository _catalogRepository { get; set; }

        [TestInitialize]
        public void  Setup() {

            var configuration = new ConfigurationBuilder()                                
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddUserSecrets("ccd05d39-4c3d-42e9-ac50-87bb97e1d73b")
                                .Build();

            _catalogRepository= new CatalogRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));            

        }

        [TestMethod()]
        public async Task GetCurrencyIdTestAsync()
        {
            bool result = true;
            bool expectedResult = true;
            Currency currency = new Currency(_catalogRepository);
            Guid id = await currency.GetCurrencyIdAsync("MXN");
            if (id == Guid.Empty)
            {
                result = false;
            }
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod()]
        public async Task CreateCurrencyAsyncTestAsync()
        {
            string expectedResult = "Item Created";
            string result = String.Empty;
            Currency currency = new Currency(_catalogRepository);
            currency.CurrencyId = Guid.NewGuid();
            currency.Description = "MXN";
            result = await currency.CreateCurrencyAsync();

            Assert.AreEqual(expectedResult,result);
        }
    }
}