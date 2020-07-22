using PoS.Sell.Domain.AggregateModels.ProductRO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface IProductRepository
    {
        Task<string> Add(AggregateModels.ProductRO.Product entity);
        Task<Product> GetById(string description);
    }
}
