using Microsoft.Azure.Cosmos;
using PoS.Sell.Domain.AggregateModels.StoreAggregate;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {

        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private string databaseId = "SellDB";
        private string containerId = "StoreContainer";

        public StoreRepository(DataStoreConfiguration dataStoreConfiguration)
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

        public async Task<string> Add(Store entity)
        {
            string result = String.Empty;

            await ConfigureDbAndContainerAsync();

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Store> StoreResponse = await _container.ReadItemAsync<Store>(entity.Key,
                                                                                                    new PartitionKey(entity.Name));
                result = "Existing Item";
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<Store> StoreResponse = await _container.CreateItemAsync<Store>(entity, new PartitionKey(entity.Name));
                result = "Item Created";

            }
            return result;
        }


        public async Task<dynamic> GetById(string id)
        {
            await ConfigureDbAndContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.id = '" + id + "'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Store> queryResultSetIterator = _container.GetItemQueryIterator<Store>(queryDefinition);


            List<Store> stores = new List<Store>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Store> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Store store in currentResultSet)
                {
                    stores.Add(store);
                }
            }

            return stores.FirstOrDefault();
        }

        
    }
}
