using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.Discount
{
    public class DiscuountBase
    {
        public string Descripcion { get; set; }
        public DateTime Vigencia { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
