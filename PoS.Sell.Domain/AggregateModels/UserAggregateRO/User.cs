using Newtonsoft.Json;
using PoS.Sell.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.AggregateModels.UserAggregate
{
    public class User
    {
        private readonly IUserRepository _userRepository;
        

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }

        public User() { }

        public User(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets the user info by Id
        /// </summary>
        /// <returns></returns>
        public User GetUserById()
        {
            User user = new User();

            return user;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            User user = new User();
            user = await _userRepository.GetById(id);
            return user;
        }

        public async Task<string> CreateUserAsync()
        {
            Id = Guid.NewGuid().ToString();
            string result = String.Empty;
            try
            {
                result = await _userRepository.Add(this);
            }
            catch (Exception e)
            {

                throw e;
            }
            return result;
        }
    }
}
