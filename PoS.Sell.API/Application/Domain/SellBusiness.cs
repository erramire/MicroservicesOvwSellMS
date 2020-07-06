using PoS.Sell.Domain.AggregateModels.SellAggregates;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoS.Sell.API.Application.Domain
{
    public class SellBusiness:ISellBusiness
    {
        private readonly ISellRepository _sellRepository;        
        private readonly IStatusSellRepository _statusSellRepository;
        private readonly IProductRepository _productRepository;
        //private readonly CC.EventBus.EventBus.IEventBus _eventBus;
        //public SellBusiness(ISellRepository sellRepository,
        //    IStatusSellRepository statusSellRepository,
        //    IProductRepository productRepository, CC.EventBus.EventBus.IEventBus eventBus)
        public SellBusiness(ISellRepository sellRepository,             
            IStatusSellRepository statusSellRepository, 
            IProductRepository productRepository) 
        {
            _sellRepository = sellRepository;            
            _statusSellRepository = statusSellRepository;
            _productRepository = productRepository;
            //_eventBus = eventBus;
        }

        public async Task<string> CreateSellAsync(string storeId, string cashdeskId,string userId, string correlationToken)
        {

            string result = String.Empty;
            PoS.Sell.Domain.AggregateModels.SellAggregates.Sell sell = 
                new PoS.Sell.Domain.AggregateModels.SellAggregates.Sell(_sellRepository, _statusSellRepository, _productRepository);
            
            sell.CashDeskID = cashdeskId;
            sell.StoreId = storeId;
            sell.UserId = userId;

            result = await sell.CreateSellAsync(correlationToken);
            return result;
            
        }


        
        public async Task<string> AddSellItemAsync(string folio_Venta, string sku, string correlationToken)
        {
            
            string result = String.Empty;
            
            PoS.Sell.Domain.AggregateModels.SellAggregates.Sell sell = 
                new PoS.Sell.Domain.AggregateModels.SellAggregates.Sell(_sellRepository, _statusSellRepository, _productRepository);


            result = await sell.AddSellItemAsync(folio_Venta, sku, correlationToken);

            return result;
        }

        
        public async Task<string> CheckOutSellAsync(string folio_Venta, string correlationToken)
        {
            
            string result = String.Empty;
            

            PoS.Sell.Domain.AggregateModels.SellAggregates.Sell sell = 
                new PoS.Sell.Domain.AggregateModels.SellAggregates.Sell(_sellRepository, _statusSellRepository, _productRepository);


            result = await sell.CheckOutSellAsync(folio_Venta, correlationToken);

            var checkout = new Sell.Domain.Events.SellCheckOutEvent
            {
                
            };

            //await _eventBus.Publish(checkout, CC.EventBus.Events.MessageEventEnum.SellCheckoutEvent, "");
            return result;
        }

    }
}
