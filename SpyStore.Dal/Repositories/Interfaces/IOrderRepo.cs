using System;
using System.Collections.Generic;
using SpyStore.Dal.Repositories.Base;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;

namespace SpyStore.Dal.Repositories.Interfaces
{
    public interface IOrderRepo : IRepo<Order>
    {
        IList<Order> GetOrderHistory();
        OrderWithDetailsAndProductInfo GetOneWithDetails(int orderId);

    }
}