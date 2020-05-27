using Microsoft.Azure.Cosmos;
using PoS.Sell.Domain.AggregateModels.Catalogs;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private string databaseId = "SellDB";
        private string containerId = "CatalogsContainer";

        public CatalogRepository(DataStoreConfiguration dataStoreConfiguration)
        {
            _endpointUri = dataStoreConfiguration.EndPointUri;
            _primaryKey = dataStoreConfiguration.Key;
            _client = new CosmosClient(_endpointUri, _primaryKey);
            
        }

        public async Task ConfigureDbAndContainerAsync() {
            _database = await _client.CreateDatabaseIfNotExistsAsync(databaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/Description");
        }

        public async Task<string> Add(Currency entity)
        {
            string result = String.Empty;

            await ConfigureDbAndContainerAsync();

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Currency> currencyResponse = await _container.ReadItemAsync<Currency>( entity.Id, 
                                                                                                    new PartitionKey(entity.Description));
                result = "Existing Item";
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                
                ItemResponse<Currency> currencyResponse = await _container.CreateItemAsync<Currency>(entity, new PartitionKey(entity.Description));
                result = "Item Created";

            }
            return result;
        }


        public async Task<dynamic> GetByDescription(string description)
        {
            await ConfigureDbAndContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.Description = '" + description + "'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Currency> queryResultSetIterator = _container.GetItemQueryIterator<Currency>(queryDefinition);

            List<Currency> currencies = new List<Currency>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Currency> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Currency currency in currentResultSet)
                {
                    currencies.Add(currency);                    
                }
            }

            return currencies.FirstOrDefault();
        }

    }
}
