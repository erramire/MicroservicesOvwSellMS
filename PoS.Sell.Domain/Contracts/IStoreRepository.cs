using PoS.Sell.Domain.AggregateModels.StoreAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface IStoreRepository
    {
        Task<string> Add(Store store);
        Task<dynamic> GetById(string key);
    }
}
