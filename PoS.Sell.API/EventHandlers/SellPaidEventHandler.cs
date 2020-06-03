using Microsoft.Azure.ServiceBus;
using PoS.CC.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoS.Sell.API.EventHandlers
{
    public class SellPaidEventHandler : IIntegratedEventHandler
    {
        public Task HandleAsync(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
