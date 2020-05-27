using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface ITaxRepository
    {
        Task<string> Add(AggregateModels.TaxRO.Tax entity);
        Task<dynamic> GetByDescription(string description);
    }
}
