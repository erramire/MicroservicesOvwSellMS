using Microsoft.Azure.Cosmos;
using PoS.Sell.Domain.AggregateModels.UserAggregate;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private string databaseId = "SellDB";
        private string containerId = "UserContainer";
        public UserRepository(DataStoreConfiguration dataStoreConfiguration)
        {
            _endpointUri = dataStoreConfiguration.EndPointUri;
            _primaryKey = dataStoreConfiguration.Key;
            _client = new CosmosClient(_endpointUri, _primaryKey);

        }

        public async Task ConfigureDbAndContainerAsync()
        {
            _database = await _client.CreateDatabaseIfNotExistsAsync(databaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/Name");
        }


        public async Task<dynamic> GetById(string id)
        {
            await ConfigureDbAndContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.id = '" + id + "'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Domain.AggregateModels.UserAggregate.User> queryResultSetIterator = _container.GetItemQueryIterator<Domain.AggregateModels.UserAggregate.User>(queryDefinition);


            List<Domain.AggregateModels.UserAggregate.User> Users = new List<Domain.AggregateModels.UserAggregate.User>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Domain.AggregateModels.UserAggregate.User> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Domain.AggregateModels.UserAggregate.User User in currentResultSet)
                {
                    Users.Add(User);
                }
            }

            return Users.FirstOrDefault();
        }

        public async Task<string> Add(Domain.AggregateModels.UserAggregate.User entity)
        {
            string result = String.Empty;

            await ConfigureDbAndContainerAsync();

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Domain.AggregateModels.UserAggregate.User> UserResponse = await _container.ReadItemAsync<Domain.AggregateModels.UserAggregate.User>(entity.Name,new PartitionKey(entity.Name));
                result = "Existing Item";
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<Domain.AggregateModels.UserAggregate.User> UserResponse = await _container.CreateItemAsync<Domain.AggregateModels.UserAggregate.User>(entity, new PartitionKey(entity.Name));
                result = "Item Created";

            }
            return result;
        }
    }
}
