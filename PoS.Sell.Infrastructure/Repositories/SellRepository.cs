using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using PoS.Sell.Domain.Contracts;
using Microsoft.Azure.Cosmos;
using System.Linq;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class SellRepository : ISellRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private string databaseId = "SellDB";
        private string containerId = "SellContainer";

        public SellRepository(DataStoreConfiguration dataStoreConfiguration)
        {
            _endpointUri = dataStoreConfiguration.EndPointUri;
            _primaryKey = dataStoreConfiguration.Key;
            _client = new CosmosClient(_endpointUri, _primaryKey);
        }

        public async Task ConfigureDbAndContainerAsync()
        {
            _database = await _client.CreateDatabaseIfNotExistsAsync(databaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/UserId");
        }

        public async Task<string> Add(Domain.AggregateModels.SellAggregates.Sell entity, string correlationToken)
        {
            string result = String.Empty;
            entity.CorrelationToken = correlationToken;
            await ConfigureDbAndContainerAsync();

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<PoS.Sell.Domain.AggregateModels.SellAggregates.Sell> SellResponse = await _container.ReadItemAsync<PoS.Sell.Domain.AggregateModels.SellAggregates.Sell>(entity.Folio_Venta,
                                                                                                    new PartitionKey(entity.UserId));
                result = "Existing Item";
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<PoS.Sell.Domain.AggregateModels.SellAggregates.Sell> SellResponse = await _container.CreateItemAsync<PoS.Sell.Domain.AggregateModels.SellAggregates.Sell>(entity, new PartitionKey(entity.UserId));
                result = "Item Created";

            }
            return result;
        }

        public async Task<string> Update(Domain.AggregateModels.SellAggregates.Sell entity, string correlationToken)
        {
            string result = String.Empty;
            entity.CorrelationToken = correlationToken;
            // replace the item with the updated content
            ItemResponse<PoS.Sell.Domain.AggregateModels.SellAggregates.Sell> SellResponse = await _container.ReplaceItemAsync<PoS.Sell.Domain.AggregateModels.SellAggregates.Sell>(entity, entity.Folio_Venta, new PartitionKey(entity.UserId));

            result = "Item Updated";

            return result;
        }

        public async Task<dynamic> GetById(string folioVenta,string correlationToken)
        {
            await ConfigureDbAndContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.id = '" + folioVenta + "'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Domain.AggregateModels.SellAggregates.Sell> queryResultSetIterator = _container.GetItemQueryIterator<Domain.AggregateModels.SellAggregates.Sell>(queryDefinition);


            List<Domain.AggregateModels.SellAggregates.Sell> sells = new List<Domain.AggregateModels.SellAggregates.Sell>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Domain.AggregateModels.SellAggregates.Sell> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Domain.AggregateModels.SellAggregates.Sell product in currentResultSet)
                {
                    sells.Add(product);
                }
            }

            return sells.FirstOrDefault();
        }
    }
}
