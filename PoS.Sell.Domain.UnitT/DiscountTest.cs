using PoS.Sell.Domain.UnitT.Discounts;
using System;
using Xunit;

namespace PoS.Sell.Domain.UnitT
{
    public class DiscountTest
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            DiscountRequest discountRequest = new DiscountRequest
            {
                Descripcion = "Descuento de chicles Trident",
                Vigencia = new DateTime(2020, 07, 31),
                Porcentaje = 10
            };


            //Act

            //Asset 


        }
    }
}
