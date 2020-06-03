using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PoS.Sell.Domain.AggregateModels.SellAggregates;

namespace PoS.Sell.Domain.Contracts
{
    public interface ISellRepository
    {
        Task<string> Add(AggregateModels.SellAggregates.Sell entity, string correlationToken);
        Task<string> Update(AggregateModels.SellAggregates.Sell entity, string correlationToken);        
        Task<dynamic> GetById(string folioVenta, string correlationToken);
    }
}
