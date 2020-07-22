using PoS.Sell.Domain.AggregateModels.Discount;
using PoS.Sell.Domain.Contracts;
using System;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Discount
{
    public class Discount : DiscuountBase
    {
        public string Id { get; set; }
        public string ProductId { get; set; }

        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;

        public Discount(IDiscountRepository discountRepository, IProductRepository productRepository)
        {
            _discountRepository = discountRepository;
            _productRepository = productRepository;
        }
        

        public async Task<DiscountResult> CreateDiscountAsync(DiscountRequest discountRequest)
        {
            if (discountRequest==null)
            {
                var ex = new ArgumentNullException("request");
                
                throw ex ;
            }

            var availableProduct = await _productRepository.GetById(discountRequest.ProductDescription);
            DiscountResult discountResult = Create<DiscountResult>(discountRequest);

            if (availableProduct!=null)
            {
                Id = Guid.NewGuid().ToString();
                Descripcion = discountRequest.Descripcion;
                Vigencia = discountRequest.Vigencia;
                Porcentaje = discountRequest.Porcentaje;
                ProductId = availableProduct.SKU;
                var result = _discountRepository.Add(this);
                discountResult.Code = DiscountResultCode.Success;
            }
            else
            {
                ProductId = "";
                discountResult.Code = DiscountResultCode.NoProductAvailable;
            }

            

            discountResult.Id = Id;
            discountResult.ProductId = ProductId;
              
            return discountResult;

        }

        private static T Create<T>(DiscountRequest discuountBase) where T :DiscuountBase, new ()
        {
           
            return new T
            {
                Descripcion = discuountBase.Descripcion,
                Vigencia = discuountBase.Vigencia,
                Porcentaje = discuountBase.Porcentaje
            }; 
        }
    }
}