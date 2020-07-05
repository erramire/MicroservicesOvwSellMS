using Newtonsoft.Json;
using PoS.Sell.Domain.AggregateModels.ProductRO;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.SellAggregates
{
    public class Sell: IAggregateRoot
    {
        private readonly ISellRepository _sellRepository;
        private readonly IStatusSellRepository _statusSellRepository;
        private readonly IProductRepository _productRepository;
        public Sell() { 
        }

        public Sell(ISellRepository sellRepository, IStatusSellRepository statusSellRepository, IProductRepository productRepository)
        {
            _sellRepository = sellRepository;
            _statusSellRepository = statusSellRepository;
            _productRepository = productRepository;
        }

        [JsonProperty(PropertyName = "id")]
        public string Folio_Venta { get; set; }
        public string StoreId{ get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public Amount Amount  { get; set; }
        public Amount Cash { get; set; }
        public int ItemsQuantity { get; set; }
        public string CashDeskID { get; set; }
        public StatusSell Status { get; set; }
        public string CorrelationToken { get; set; }

        public Dictionary<string,SellItem> SellItems;



        

        public async Task<string> CreateSellAsync(string correlationToken)
        {
            string result = String.Empty;
            try
            {
                Folio_Venta = Guid.NewGuid().ToString();
                StatusSell statusSell = new StatusSell(_statusSellRepository);
                Status = await statusSell.GetStatusSellByDesc("Activa");
                SellItems = new Dictionary<string, SellItem>();
                Date = DateTime.UtcNow;
                ItemsQuantity = 0;
                result = await _sellRepository.Add(this, correlationToken);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }

        public async Task<string> AddSellItemAsync(string folio_Venta, string sku, string correlationToken)
        {
            string result = String.Empty;
            try
            {

                await GetSellbyIdAsync(folio_Venta,correlationToken);

                await AddItemToSellAsync(sku, correlationToken);
                                
                ItemsQuantity += 1;
                
                result = await _sellRepository.Update(this, correlationToken);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }

        private async Task AddItemToSellAsync(string sku, string correlationToken)
        {
            if (SellItems.ContainsKey(sku))
            {
                SellItems[sku].AddQuantityItemSold();
            }
            else {
                Product product = new Product(_productRepository);
                product = await product.GetProductBySKUAsync(sku);

                SellItem sellItem = new SellItem(product, 1);
                SellItems.Add(sku, sellItem);
            }

            CalculateTotalAmount();
            

        }

        private void CalculateTotalAmount() {
            decimal total = 0;
            Guid currencyId = Guid.Empty;
            foreach (var item in SellItems)
            {
                total += item.Value.TotalPaid.TotalAmount;
                currencyId = item.Value.TotalPaid.CurrencyId;
            }

            Amount = new Amount(currencyId, total);

        }

        public async Task<string> CheckOutSellAsync(string folio_Venta, string correlationToken)
        {
            string result = String.Empty;
            try
            {

                await GetSellbyIdAsync(folio_Venta,correlationToken);

                StatusSell statusSell = new StatusSell(_statusSellRepository);
                statusSell = await statusSell.GetStatusSellByDesc("Checkout");
                Status = statusSell;

                result = await _sellRepository.Update(this, correlationToken);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }

        private async Task GetSellbyIdAsync(string folio_Venta, string correlationToken)
        {
            Sell sell = new Sell();
            sell = await _sellRepository.GetById(folio_Venta, correlationToken);
            InitilizeEntityProperties(sell);
            
        }

        private void InitilizeEntityProperties(Sell sell)
        {
            Folio_Venta = sell.Folio_Venta;
            StoreId = sell.StoreId;
            UserId = sell.UserId;
            Date = sell.Date;
            Amount = sell.Amount;
            Cash = sell.Cash;
            ItemsQuantity = sell.ItemsQuantity;
            CashDeskID = sell.CashDeskID;
            Status = sell.Status;
            SellItems = sell.SellItems;
        }
    }
}
