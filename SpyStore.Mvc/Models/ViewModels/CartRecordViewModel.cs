using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SpyStore.Models.ViewModels;
using SpyStore.Mvc.Validation;

namespace SpyStore.Mvc.Models.ViewModels
{
    /// <summary>
    /// The ViewModel will be used to update the cart and, as such, allows zero to be
    /// entered(indicating removal of the item from the cart).
    /// </summary>
    public class CartRecordViewModel : CartRecordWithProductInfo
    {
        [Required]
        [MustNotBeGreaterThan(nameof(UnitsInStock))]
        public new int Quantity { get; set; }
    }
}
