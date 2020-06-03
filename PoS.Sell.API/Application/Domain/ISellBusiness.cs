using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoS.Sell.API.Application.Domain
{
    public interface ISellBusiness
    {
        Task<string> CreateSellAsync(string storeId, string cashdeskId, string userId, string correlationToken);
        Task<string> AddSellItemAsync(string folio_Venta, string sku, string correlationToken);
        Task<string> CheckOutSellAsync(string folio_Venta, string correlationToken);
    }
}
