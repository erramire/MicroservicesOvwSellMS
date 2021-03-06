﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoS.Sell.Domain.Contracts
{
    public interface IStatusSellRepository
    {
        Task<string> Add(AggregateModels.SellAggregates.StatusSell entity);
        Task<dynamic> GetByDesc(string description);
    }
}
