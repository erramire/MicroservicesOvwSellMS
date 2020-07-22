using Moq;
using PoS.Sell.Domain.AggregateModels.Discount;
using PoS.Sell.Domain.AggregateModels.ProductRO;
using PoS.Sell.Domain.AggregateModels.TaxRO;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using PoS.Sell.Domain.Contracts;
using PoS.Sell.Domain.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace PoS.Sell.Domain.UnitT
{
    public class DiscountTest
    {
        private readonly DiscountRequest discountRequest;
        private readonly PoS.Sell.Domain.Discount.Discount discount;
        private readonly Mock<IDiscountRepository> _discountRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly List<Product> _availableproduct;

        public DiscountTest()
        {
            //Arrange
            _discountRepositoryMock = new Mock<IDiscountRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            discount = new PoS.Sell.Domain.Discount.Discount(_discountRepositoryMock.Object, _productRepositoryMock.Object);
            _availableproduct = new List<Product>();
            _availableproduct = new List<Product>{
                new Product()
                {
                    Description="Chicles Trident 18's",
                    SKU="7506105606053",
                    Price = new Amount()
                    {
                        CurrencyId=Guid.Parse("50bca81b-4099-4087-8469-874fdc6906cf"),
                        TotalAmount=14
                    },
                    Taxes= new List<Tax>
                    {
                        new Tax()
                        {
                            Id="b0d92892-eb04-4a34-a655-63264cc923eb",
                            Percentage=16,
                            Description="IVA"
                        }
                    }
                }
            };
            discountRequest = new DiscountRequest
            {
                ProductDescription = "Chicles Trident 18's",
                Descripcion = "Descuento de chicles Trident",
                Vigencia = new DateTime(2020, 07, 31),
                Porcentaje = 10
            };
        }

        [Fact]
        public async Task ShouldReturnDiscountCreatedWithValuesAsync()
        {

            //Act

            DiscountResult result = await discount.CreateDiscountAsync(discountRequest);


            //Asset 
            Assert.NotNull(result);
            Assert.Equal(discountRequest.Descripcion, result.Descripcion);
            Assert.Equal(discountRequest.Porcentaje, result.Porcentaje);
            Assert.Equal(discountRequest.Vigencia, result.Vigencia);


        }

        [Fact]
        public async Task ShouldThrowExceptionIfRequestIsNull()
        {
            //Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => discount.CreateDiscountAsync(null));

            //Asset
            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task ShouldSaveDiscountObject()
        {
            //Arrange

            PoS.Sell.Domain.Discount.Discount saveDiscount = null;


            _discountRepositoryMock.Setup(x => x.Add(It.IsAny<Discount.Discount>())).Callback<Discount.Discount>(discount =>
            {
                saveDiscount = discount;
            });



            _productRepositoryMock.Setup(x => x.GetById(discountRequest.ProductDescription)).ReturnsAsync(_availableproduct.FirstOrDefault());




            //Act
            var result = await discount.CreateDiscountAsync(discountRequest);
            _discountRepositoryMock.Verify(x => x.Add(It.IsAny<Discount.Discount>()), Times.Once);

            //Assert
            Assert.NotNull(saveDiscount);
            Assert.NotNull(saveDiscount.Id);
            Assert.Equal(discountRequest.Descripcion, saveDiscount.Descripcion);
            Assert.Equal(discountRequest.Porcentaje, saveDiscount.Porcentaje);
            Assert.Equal(discountRequest.Vigencia, saveDiscount.Vigencia);


        }

        [Theory]
        [InlineData("7506105606053", true)]
        [InlineData("", false)]
        public async Task ShouldReturnExpectedProductAsync(string expectedProductId, bool isProductAvailable)
        {
            //Arrange            

            if (!isProductAvailable)
            {
                _availableproduct.Clear();
            }

            _productRepositoryMock.Setup(x => x.GetById(discountRequest.ProductDescription)).ReturnsAsync(_availableproduct.FirstOrDefault());


            //Act
            DiscountResult result = await discount.CreateDiscountAsync(discountRequest);

            //Assert
            Assert.Equal(expectedProductId, result.ProductId);

        }

        [Theory]
        [InlineData(DiscountResultCode.Success, true)]
        [InlineData(DiscountResultCode.NoProductAvailable, false)]
        [InlineData(DiscountResultCode.NoProductAvailable, true)]
        public async Task ShouldReturnExpectedResultoCodeAsync(DiscountResultCode expectedResultCode, bool isProductAvailable) {
            //Arrange            

            if (!isProductAvailable)
            {
                _availableproduct.Clear();
            }

            _productRepositoryMock.Setup(x => x.GetById(discountRequest.ProductDescription)).ReturnsAsync(_availableproduct.FirstOrDefault());

            //Act
            DiscountResult result = await discount.CreateDiscountAsync(discountRequest);

            //Assert 
            Assert.Equal(expectedResultCode, result.Code);

        }

    }
}
