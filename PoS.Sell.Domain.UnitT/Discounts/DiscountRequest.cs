using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.UnitT.Discounts
{
    public class DiscountRequest
    {
        public string Descripcion { get; set; }
        public DateTime Vigencia { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
