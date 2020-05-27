using Microsoft.Azure.Cosmos;
using PoS.Sell.Domain.AggregateModels.Catalogs;
using PoS.Sell.Domain.AggregateModels.ProductRO;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private string databaseId = "SellDB";
        private string containerId = "ProductContainer";

        public ProductRepository(DataStoreConfiguration dataStoreConfiguration)
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

        public async Task<string> Add(Product entity)
        {
            string result = String.Empty;

            await ConfigureDbAndContainerAsync();

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Product> ProductResponse = await _container.ReadItemAsync<Product>(entity.SKU,
                                                                                                    new PartitionKey(entity.Description));
                result = "Existing Item";
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<Product> ProductResponse = await _container.CreateItemAsync<Product>(entity, new PartitionKey(entity.Description));
                result = "Item Created";

            }
            return result;
        }


        public async Task<dynamic> GetById(string id)
        {
            await ConfigureDbAndContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.id = '" + id + "'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Product> queryResultSetIterator = _container.GetItemQueryIterator<Product>(queryDefinition);


            List<Product> products = new List<Product>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Product> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Product product in currentResultSet)
                {
                    products.Add(product);
                }
            }

            return products.FirstOrDefault();
        }
        
    }
}
