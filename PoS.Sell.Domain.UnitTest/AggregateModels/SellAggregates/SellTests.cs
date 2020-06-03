using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoS.Sell.Domain.AggregateModels.Catalogs;
using PoS.Sell.Domain.AggregateModels.SellAggregates;
using PoS.Sell.Domain.AggregateModels.StoreAggregate;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.SellAggregates.Tests
{
    [TestClass()]
    public class SellTests
    {
        private ISellRepository _sellRepository;
        private IStoreRepository _storeRepository;
        private IStatusSellRepository _statusSellRepository;
        private IProductRepository _productRepository;

        [TestInitialize]
        public void Setup()
        {

            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddUserSecrets("ccd05d39-4c3d-42e9-ac50-87bb97e1d73b")
                                .Build();

            _sellRepository = new SellRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            
            _storeRepository = new StoreRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));

            _statusSellRepository = new StatusSellRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));

            _productRepository= new ProductRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));



        }

        [TestMethod()]
        public async Task CreateSellTestAsync()
        {
            string expectedResult = "Item Created";
            string result = String.Empty;
            Store store = new Store(_storeRepository);
            store = await store.GetStoreByKeyAsync("ST01");
            string cashdeskId = store.CashDesks.Where(x=>x.Description=="Caja 1").FirstOrDefault().Id;

            Sell sell = new Sell(_sellRepository, _statusSellRepository,_productRepository);
            sell.CashDeskID = cashdeskId;
            sell.StoreId = "ST01";
            sell.UserId = "2a891b52-cfae-4d3d-a1ea-94440b7983c8";
            
            result = await sell.CreateSellAsync(Guid.NewGuid().ToString());

            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod()]
        public async Task AddSellItemTestAsync()
        {
            string expectedResult = "Item Updated";
            string result = String.Empty;
            string folio_Venta = "b9ebd1a6-12a4-4834-8172-f26466bc60d9";
            string sku = "7506105606053";

            Sell sell = new Sell(_sellRepository,_statusSellRepository,_productRepository);
            

            result = await sell.AddSellItemAsync(folio_Venta,sku, Guid.NewGuid().ToString());

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public async Task CheckOutSellTestAsync()
        {
            string expectedResult = "Item Updated";
            string result = String.Empty;
            string folio_Venta = "b9ebd1a6-12a4-4834-8172-f26466bc60d9";            

            Sell sell = new Sell(_sellRepository, _statusSellRepository, _productRepository);


            result = await sell.CheckOutSellAsync(folio_Venta, Guid.NewGuid().ToString());

            Assert.AreEqual(expectedResult, result);
        }

    }
}