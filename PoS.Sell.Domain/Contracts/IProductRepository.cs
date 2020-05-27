using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface IProductRepository
    {
        Task<string> Add(AggregateModels.ProductRO.Product entity);
        Task<dynamic> GetById(string description);
    }
}
