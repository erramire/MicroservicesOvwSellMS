using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface IUserRepository
    {
        Task<string> Add(AggregateModels.UserAggregate.User entity);
        Task<dynamic> GetById(string description);
    }
}
