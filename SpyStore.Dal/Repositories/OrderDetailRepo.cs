using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Repositories.Base;
using SpyStore.Dal.Repositories.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;

namespace SpyStore.Dal.Repositories
{
    public class OrderDetailRepo : RepoBase<OrderDetail>, IOrderDetailRepo
    {
        public OrderDetailRepo(StoreContext context) : base(context)
        {
        }
        internal OrderDetailRepo(DbContextOptions<StoreContext> options)
        : base(options)
        {
        }

        public IEnumerable<OrderDetailWithProductInfo> GetOrderDetailsWithProductInfoForOrder(int orderId)
            => Context
                .OrderDetailWithProductInfos
                .Where(o => o.OrderId == orderId)
                .OrderBy(o => o.ModelName);

    }
}
