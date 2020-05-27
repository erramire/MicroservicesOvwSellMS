using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoS.Sell.Domain.AggregateModels.StoreAggregate;
using PoS.Sell.Domain.AggregateModels.StoreAggregateRO;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.StoreAggregate.Tests
{
    [TestClass()]
    public class StoreTests
    {
        private IStoreRepository _StoreRepository;

        [TestInitialize]
        public void Setup()
        {

            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddUserSecrets("ccd05d39-4c3d-42e9-ac50-87bb97e1d73b")
                                .Build();

            _StoreRepository = new StoreRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            

        }

        [TestMethod()]
        public async Task GetStoreBySKUTestAsync()
        {
            bool result = true;
            bool expectedResult = true;
            Store Store = new Store(_StoreRepository);
            Store StoreResult = await Store.GetStoreByKeyAsync("ST01");
            if (StoreResult == null)
            {
                result = false;
            }
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public async Task CreateStoreAsyncTestAsync()
        {
            string expectedResult = "Item Created";
            string result = String.Empty;
            Store store = new Store(_StoreRepository);
            store.Key = "ST01";
            store.Name = "Tienda 1";
            
            CashDesk cashDesk = new CashDesk("ST01C1","Caja 1");            
            List<CashDesk> cashDesks = new List<CashDesk>();
            cashDesks.Add(cashDesk);
            store.CashDesks=cashDesks;

            result = await store.CreateStoreAsync();

            Assert.AreEqual(expectedResult, result);
        }

        
    }
}