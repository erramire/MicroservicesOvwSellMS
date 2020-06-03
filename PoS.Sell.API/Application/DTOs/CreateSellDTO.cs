using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoS.Sell.API.Application.DTOs
{
    public class CreateSellDTO
    {
        public string StoreId { get; set; }
        public string CashdeskId { get; set; }
        public string CashierId { get; set; }
    }
}
