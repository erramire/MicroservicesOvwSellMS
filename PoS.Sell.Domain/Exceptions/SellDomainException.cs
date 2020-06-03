using System;
using System.Collections.Generic;
using System.Text;

namespace PoS.Sell.Domain.Exceptions
{
    public class SellDomainException:Exception
    {
        public SellDomainException()
        {
        }

        public SellDomainException(string message)
            : base(message)
        {
        }

        public SellDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
