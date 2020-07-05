using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using PoS.CC.EventBus.Events;
using PoS.Sell.Domain.AggregateModels.ProductRO;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoS.Sell.API.EventHandlers
{
    public class ProductChangedEventHandler : IIntegratedEventHandler
    {
        public readonly IProductRepository productRepository;

        public ProductChangedEventHandler(IProductRepository _productRepository) {
            productRepository = _productRepository;
        }
        public async Task HandleAsync(Message message)
        {
            
            Product product = new Product(productRepository);
            var mesaggeProduct = JsonConvert.DeserializeObject<ProductChangedEvent>(message.Body.ToString());

            product.Description = mesaggeProduct.Description;
            product.SKU = mesaggeProduct.SKU;
            product.Price = mesaggeProduct.Price;

            var result = await product.CreateProductAsync();

            ///await product.CreateProductAsync();
            throw new NotImplementedException();
        }
    }
}
