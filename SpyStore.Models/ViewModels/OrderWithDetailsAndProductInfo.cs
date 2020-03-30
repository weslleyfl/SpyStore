﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper;
using SpyStore.Models.Entities;
using SpyStore.Models.Entities.Base;

namespace SpyStore.Models.ViewModels
{
    public class OrderWithDetailsAndProductInfo : OrderBase
    {
        private static readonly MapperConfiguration _mapperCfg;
        public Customer Customer { get; set; }
        public IList<OrderDetailWithProductInfo> OrderDetails { get; set; }

        static OrderWithDetailsAndProductInfo()
        {
            _mapperCfg = new MapperConfiguration(cfg =>
            {
                // In this case, the mapping will not copy the
                // OrderDetails property from the source to the target.
                cfg.CreateMap<Order, OrderWithDetailsAndProductInfo>()
                   .ForMember(record => record.OrderDetails, y => y.Ignore());
            });
        }

        public static OrderWithDetailsAndProductInfo Create(Order order, Customer customer, IEnumerable<OrderDetailWithProductInfo> details)
        {
            var viewModel = _mapperCfg.CreateMapper().Map<OrderWithDetailsAndProductInfo>(order);
            viewModel.OrderDetails = details.ToList();
            viewModel.Customer = customer;

            return viewModel;
        }


    }
}
