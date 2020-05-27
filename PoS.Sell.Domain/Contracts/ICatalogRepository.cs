using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface ICatalogRepository
    {
        Task<string> Add(AggregateModels.Catalogs.Currency entity);        
        Task<dynamic> GetByDescription(string description);
    }
}
