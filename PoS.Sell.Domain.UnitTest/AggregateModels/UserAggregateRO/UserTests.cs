using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoS.Sell.Domain.AggregateModels.UserAggregate;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.UserAggregate.Tests
{
    [TestClass()]
    public class UserTests
    {
        private IUserRepository _userRepository;

        [TestInitialize]
        public void Setup()
        {

            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddUserSecrets("ccd05d39-4c3d-42e9-ac50-87bb97e1d73b")
                                .Build();

            _userRepository = new UserRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            

        }


        [TestMethod()]
        public async Task CreateUserAsyncTestAsync()
        {
            string expectedResult = "Item Created";
            string result = String.Empty;
            User User = new User(_userRepository);            
            User.Name = "Erick Ramírez";            

            result = await User.CreateUserAsync();

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        public async Task GetUserByIdTestAsync()
        {
            bool result = true;
            bool expectedResult = true;
            User User = new User(_userRepository);
            User UserResult = await User.GetUserByIdAsync("2a891b52-cfae-4d3d-a1ea-94440b7983c8");
            if (UserResult == null)
            {
                result = false;
            }
            Assert.AreEqual(expectedResult, result);
        }
    }
}