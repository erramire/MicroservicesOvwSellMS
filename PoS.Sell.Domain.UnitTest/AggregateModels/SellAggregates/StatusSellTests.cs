using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoS.Sell.Domain.AggregateModels.SellAggregates;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.SellAggregates.Tests
{
    [TestClass()]
    public class StatusSellTests
    {
        private IStatusSellRepository _statusSellRepository;

        [TestInitialize]
        public void Setup()
        {

            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddUserSecrets("ccd05d39-4c3d-42e9-ac50-87bb97e1d73b")
                                .Build();

            _statusSellRepository = new StatusSellRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));            

        }

        [TestMethod()]
        public async Task GetStatusSellByDescTestAsync()
        {
            bool result = true;
            bool expectedResult = true;
            StatusSell StatusSell = new StatusSell(_statusSellRepository);
            StatusSell StatusSellResult = await StatusSell.GetStatusSellByDesc("Activa");
            if (StatusSellResult == null)
            {
                result = false;
            }
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public async Task CreateStatusSellAsyncTestAsync()
        {
            string expectedResult = "Item Created";
            string result = String.Empty;
            StatusSell StatusSell = new StatusSell(_statusSellRepository);
            StatusSell.Description = "Pagada";

            result = await StatusSell.CreateStatusSellAsync();

            Assert.AreEqual(expectedResult, result);
        }
    }
}