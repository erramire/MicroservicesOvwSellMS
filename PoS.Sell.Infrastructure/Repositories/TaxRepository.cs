using Microsoft.Azure.Cosmos;
using PoS.Sell.Domain.AggregateModels.TaxRO;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class TaxRepository : ITaxRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private string databaseId = "SellDB";
        private string containerId = "TaxContainer";

        public TaxRepository(DataStoreConfiguration dataStoreConfiguration)
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

        public async Task<string> Add(Tax entity)
        {
            string result = String.Empty;

            await ConfigureDbAndContainerAsync();

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Tax> currencyResponse = await _container.ReadItemAsync<Tax>(entity.Id,
                                                                                                    new PartitionKey(entity.Description));
                result = "Existing Item";
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<Tax> currencyResponse = await _container.CreateItemAsync<Tax>(entity, new PartitionKey(entity.Description));
                result = "Item Created";

            }
            return result;
        }


        public async Task<dynamic> GetByDescription(string description)
        {
            await ConfigureDbAndContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.Description = '" + description + "'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Tax> queryResultSetIterator = _container.GetItemQueryIterator<Tax>(queryDefinition);

            List<Tax> taxes = new List<Tax>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Tax> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Tax tax in currentResultSet)
                {
                    taxes.Add(tax);
                }
            }

            return taxes.FirstOrDefault();
        }

    }
    
}
