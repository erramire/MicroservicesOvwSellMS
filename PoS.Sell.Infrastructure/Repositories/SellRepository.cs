using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Net;
using PoS.Sell.Domain.Contracts;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class SellRepository : ISellRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private DocumentClient _client;
                
        public SellRepository(DataStoreConfiguration dataStoreConfiguration)
        {
            _endpointUri = dataStoreConfiguration.EndPointUri;
            _primaryKey = dataStoreConfiguration.Key;
        }

        public async Task<string> Add(Domain.AggregateModels.SellAggregates.Sell entity)
        {
            _client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = "SellDB" });

            //TODO: Add idempotent write check. Ensure that update with same correlation token does not already exist. 

            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("SellDB"),
                new DocumentCollection { Id = "SellCollection" });
            return await CreateSellDocumentIfNotExists("SellDB", "SellCollection", entity);
        }


        public Task<string> Delete(Domain.AggregateModels.SellAggregates.Sell entity)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetAll(string SellId, string correlationToken)
        {
            dynamic Sell = null;

            try
            {
                _client = new DocumentClient(new Uri(_endpointUri), _primaryKey);

                var response =
                    await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri("SellDB", "SellCollection",
                        SellId));
                Sell = response.Resource;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    Sell = null;

                // Cannot find specified document
            }
            catch (Exception ex)
            {
                //TODO: Log Error
                throw;
            }

            return Sell;
        }

        public async Task<dynamic> GetById(string correlationToken)
        {
            var Sells = new List<dynamic>();
            try
            {
                dynamic Sell = null;
                _client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
                var documents =
                    await _client.ReadDocumentFeedAsync(
                        UriFactory.CreateDocumentCollectionUri("SellDB", "SellCollection"));

                foreach (Document document in documents)
                {
                    Sell = document;
                    Sells.Add(Sell);
                }
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    Sells = null;

                // Cannot find specified document
            }
            catch (Exception ex)
            {
                //TODO: Log Error
                throw;
            }

            return Sells;
        }

        public Task<bool> Update(Domain.AggregateModels.SellAggregates.Sell entity)
        {
            throw new NotImplementedException();
        }

        private async Task<string> CreateSellDocumentIfNotExists(string databaseName, string collectionName,
            PoS.Sell.Domain.AggregateModels.SellAggregates.Sell Sells)
        {
            try
            {
                var response = await _client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    Sells);

                return response.Resource.Id;
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    // await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), Sells);
                }
                else
                {
                    throw;
                }
            }

            return null;
        }
    }
}
