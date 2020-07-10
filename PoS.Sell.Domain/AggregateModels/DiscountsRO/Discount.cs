using PoS.Sell.Domain.Contracts;
using System;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.DiscountsRO
{
    public class Discount : DiscountBase
    {
        private readonly IDiscountRepository _discountRepository;
        private IProductRepository _paymentRepository;

        public string Id { get; set; }
        public string ProductIds { get; set; }

        public Discount()
        {
        }

        public Discount(IDiscountRepository discountRepository, IProductRepository paymentRepository)
        {
            _discountRepository = discountRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<DiscountResult> CreateDiscountAsync(DiscountRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var availableProduct = await _paymentRepository.GetById(request.ProductIds);

            Discount discount = Create<Discount>(request);
            DiscountResult discountResult = Create<DiscountResult>(request);

            if (availableProduct != null)
            {
                discount.ProductIds = availableProduct.SKU;
                discount.Id = Guid.NewGuid().ToString();
                var result = await _discountRepository.Add(discount);
                discountResult.Id = discount.Id;
                discountResult.ProductIds = discount.ProductIds;
                discountResult.Code = DiscountResultCode.Success;
            }
            else
            {
                discountResult.ProductIds = "";
                discountResult.Code = DiscountResultCode.NoProductAvailable;
            }

            return discountResult;
        }

        private static T Create<T>(DiscountRequest request) where T : DiscountBase, new()
        {
            return new T
            {
                Percentage = request.Percentage,
                Description = request.Description,
                ValidDate = request.ValidDate
            };

        }
    }
}