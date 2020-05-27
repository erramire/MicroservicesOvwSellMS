using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PoS.Sell.Domain.AggregateModels.SellAggregates;

namespace PoS.Sell.Domain.Contracts
{
    public interface ISellRepository
    {
        Task<string> Add(AggregateModels.SellAggregates.Sell entity);
        Task<bool> Update(AggregateModels.SellAggregates.Sell entity);
        Task<string> Delete(AggregateModels.SellAggregates.Sell entity);
        Task<dynamic> GetAll(string orderId, string correlationToken);
        Task<dynamic> GetById(string correlationToken);
    }
}
