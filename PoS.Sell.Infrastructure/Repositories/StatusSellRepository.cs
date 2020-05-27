using Microsoft.Azure.Cosmos;
using PoS.Sell.Domain.AggregateModels.SellAggregates;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class StatusSellRepository : IStatusSellRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private string databaseId = "SellDB";
        private string containerId = "StatusSellContainer";
        

        public StatusSellRepository(DataStoreConfiguration dataStoreConfiguration)
        {
            _endpointUri = dataStoreConfiguration.EndPointUri;
            _primaryKey = dataStoreConfiguration.Key;
            _client = new CosmosClient(_endpointUri, _primaryKey);

        }

        public async Task ConfigureDbAndContainerAsync()
        {
            _database = await _client.CreateDatabaseIfNotExistsAsync(databaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/Description");
        }

        public async Task<string> Add(StatusSell entity)
        {
            string result = String.Empty;

            await ConfigureDbAndContainerAsync();

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<StatusSell> StatusSellResponse = await _container.ReadItemAsync<StatusSell>(entity.Id, new PartitionKey(entity.Description));
                result = "Existing Item";
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<StatusSell> StatusSellResponse = await _container.CreateItemAsync<StatusSell>(entity, new PartitionKey(entity.Description));
                result = "Item Created";

            }
            return result;
        }

        public async Task<dynamic> GetByDesc(string description)
        {
            await ConfigureDbAndContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.Description = '" + description + "'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<StatusSell> queryResultSetIterator = _container.GetItemQueryIterator<StatusSell>(queryDefinition);


            List<StatusSell> StatusSells = new List<StatusSell>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<StatusSell> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (StatusSell StatusSell in currentResultSet)
                {
                    StatusSells.Add(StatusSell);
                }
            }

            return StatusSells.FirstOrDefault();
        }
    }
}
