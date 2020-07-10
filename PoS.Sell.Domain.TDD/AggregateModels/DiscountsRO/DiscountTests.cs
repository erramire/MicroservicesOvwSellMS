using Moq;
using PoS.Sell.Domain.AggregateModels.ProductRO;
using PoS.Sell.Domain.AggregateModels.TaxRO;
using PoS.Sell.Domain.AggregateModels.ValueObject;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PoS.Sell.Domain.AggregateModels.DiscountsRO
{
    public class DiscountTests
    {
        private readonly DiscountRequest _request;
        private readonly List<Product> _availableProducts;
        private readonly Mock<IDiscountRepository> _discountRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Discount _discount;

        public DiscountTests()
        {
            _request = new DiscountRequest
            {
                Percentage = 10,
                Description = "10% de descuento en chicles trident 18's",
                ProductIds = "7506105606053",
                ValidDate = new DateTime(2020, 07, 24)
            };

            _availableProducts = new List<Product>{
                new Product()
                {
                    Description="",
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

            _discountRepositoryMock = new Mock<IDiscountRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();


            _discount = new Discount(_discountRepositoryMock.Object, _productRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnDiscountCreatedWithResultValuesAsync()
        {

            //Act
            DiscountResult result = await _discount.CreateDiscountAsync(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_request.Percentage, result.Percentage);
            Assert.Equal(_request.Description, result.Description);
            Assert.Equal(_request.ValidDate, result.ValidDate);
        }

        [Fact]
        public async Task ShouldThrowExceptionIfRequestIsNull()
        {
            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _discount.CreateDiscountAsync(null));

            //Assert
            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task ShouldSaveDiscountObjectAsync()
        {
            Discount savedDiscount = null;

            _productRepositoryMock.Setup(x => x.GetById(_request.ProductIds))
                .ReturnsAsync(_availableProducts.Where(x => x.SKU == _request.ProductIds).FirstOrDefault());

            _discountRepositoryMock.Setup(x => x.Add(It.IsAny<Discount>()))
                .Callback<Discount>(discount =>
                {
                    savedDiscount = discount;
                });

            var result = await _discount.CreateDiscountAsync(_request);
            _discountRepositoryMock.Verify(x => x.Add(It.IsAny<Discount>()), Times.Once);

            Assert.NotNull(savedDiscount);
            Assert.Equal(result.Id,savedDiscount.Id);
            Assert.Equal(_request.Description, savedDiscount.Description);
            Assert.Equal(_request.Percentage, savedDiscount.Percentage);
            Assert.Equal(_request.ProductIds, savedDiscount.ProductIds);
            Assert.Equal(_request.ValidDate, savedDiscount.ValidDate);
        }

        [Fact]
        public async Task ShouldNotSaveDiscountIfNoProductIsAvailable()
        {
            _availableProducts.Clear();
            _productRepositoryMock.Setup(x => x.GetById(_request.ProductIds))
                .ReturnsAsync(_availableProducts.Where(x => x.SKU == _request.ProductIds).FirstOrDefault());

            var result = await _discount.CreateDiscountAsync(_request);
            _discountRepositoryMock.Verify(x => x.Add(It.IsAny<Discount>()), Times.Never);
        }

        [Theory]
        [InlineData(DiscountResultCode.Success, true)]
        [InlineData(DiscountResultCode.NoProductAvailable, false)]
        public async Task ShouldReturnExpectedResultCodeAsync(DiscountResultCode expectedResultCode, bool isProductAvailable)
        {
            if (!isProductAvailable)
            {
                _availableProducts.Clear();

            }

            _productRepositoryMock.Setup(x => x.GetById(_request.ProductIds))
                    .ReturnsAsync(_availableProducts.Where(x => x.SKU == _request.ProductIds).FirstOrDefault());

            var result = await _discount.CreateDiscountAsync(_request);

            Assert.Equal(expectedResultCode, result.Code);

        }
        
        [Theory]
        [InlineData("7506105606053", true)]
        [InlineData("", false)]
        public async Task ShouldReturnExpectedProductIdAsync(string expectedProductId, bool isProductAvailable)
        {
            if (!isProductAvailable)
            {
                _availableProducts.Clear();

            }

            _productRepositoryMock.Setup(x => x.GetById(_request.ProductIds))
                    .ReturnsAsync(_availableProducts.Where(x => x.SKU == _request.ProductIds).FirstOrDefault());

            var result = await _discount.CreateDiscountAsync(_request);

            Assert.Equal(expectedProductId, result.ProductIds);

        }
    }
}
