using PoS.CC.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Infrastructure.Repositories
{
    public class DataStoreConfiguration
    {
        public DataStoreConfiguration(string endPointUri, string key)
        {
            Guard.ForNullOrEmpty(endPointUri, "Falta la Uri de CosmosDB");
            Guard.ForNullOrEmpty(key, "falta el Key de CosmosDB");
            EndPointUri = endPointUri;
            Key = key;
        }

        public string EndPointUri { get; set; }
        public string Key { get; set; }
    }
}
