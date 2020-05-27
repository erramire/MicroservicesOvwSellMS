using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.AggregateModels.UserAggregate
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; }

        public User() { }

        /// <summary>
        /// Gets the user info by Id
        /// </summary>
        /// <returns></returns>
        public User GetUserById()
        {
            User user = new User();

            return user;
        }
    }
}
