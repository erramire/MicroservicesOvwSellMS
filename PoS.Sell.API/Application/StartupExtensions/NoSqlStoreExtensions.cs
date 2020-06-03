using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoS.Sell.API.Application.StartupExtensions
{
    public static class NoSqlStoreExtensions
    {
        public static IServiceCollection RegisterNoSqlStore(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register repository class. Note how we pass connection information
            services.AddScoped<ISellRepository, SellRepository>(x =>
            {
                return new SellRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            });

            services.AddScoped<IStoreRepository, StoreRepository>(x =>
            {
                return new StoreRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            });

            services.AddScoped<IStatusSellRepository, StatusSellRepository>(x =>
            {
                return new StatusSellRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            });

            services.AddScoped<IProductRepository, ProductRepository>(x =>
            {
                return new ProductRepository(
                    new DataStoreConfiguration(
                        configuration["CosmosEndpoint"],
                        configuration["CosmosPrimaryKey"]));
            });

            return services;
        }
    }
}
