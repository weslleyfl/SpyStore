﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SpyStore.Models.ViewModels;
using SpyStore.Mvc.Validation;

namespace SpyStore.Mvc.Models.ViewModels
{
    public class AddToCartViewModel : CartRecordWithProductInfo
    {
        [Required]
        [MustNotBeGreaterThan(nameof(UnitsInStock))]
        [MustBeGreaterThanZero]
        public new int Quantity { get; set; }
    }
}
