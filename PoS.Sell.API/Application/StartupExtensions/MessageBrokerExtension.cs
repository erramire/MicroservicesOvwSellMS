using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoS.CC.EventBus.Connection;
using PoS.CC.EventBus.EventBus;
using PoS.CC.EventBus.Events;
using PoS.CC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoS.Sell.API.Application.StartupExtensions
{
    public static class MessageBrokerExtension
    {
        public static IServiceCollection RegisterMessageBroker(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["ServiceBusPublisherConnectionString"];
            var topicName = configuration["ServiceBusTopicName"];
            var subscriptionName = configuration["ServiceBusSubscriptionName"];

            Guard.ForNullOrEmpty(connectionString, "ConnectionString from Sell is Null");
            Guard.ForNullOrEmpty(topicName, "TopicName from Sell is Null");
            Guard.ForNullOrEmpty(subscriptionName, "SubscriptionName from Sell is Null");

            // Add EventHandlers to DI Container
            services.AddTransient<IIntegratedEventHandler, EventHandlers.SellPaidEventHandler>();

            // Add Service Bus Connection Object to DI Container
            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var serviceBusConnectionString = connectionString;
                var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);
                return new DefaultServiceBusPersisterConnection(serviceBusConnection);
            });

            // Add EventBus to DI Container
            services.AddSingleton<IEventBus, EventBusServiceBus>(x =>
            {
                var serviceBusPersisterConnection = x.GetRequiredService<IServiceBusPersisterConnection>();
                return new EventBusServiceBus(serviceBusPersisterConnection, subscriptionName);
            });

            return services;
        }
    }
}
