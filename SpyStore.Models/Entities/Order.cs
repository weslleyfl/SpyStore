﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using SpyStore.Models.Entities.Base;

namespace SpyStore.Models.Entities
{
    [Table("Orders", Schema = "Store")]
    public class Order : OrderBase
    {
        [ForeignKey(nameof(CustomerId))]
        public Customer CustomerNavigation { get; set; }

        [InverseProperty(nameof(OrderDetail.OrderNavigation))]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
