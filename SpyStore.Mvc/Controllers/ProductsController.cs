using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpyStore.Mvc.Controllers.Base;
using SpyStore.Mvc.Support;
using SpyStore.Mvc.Models.ViewModels;
using SpyStore.Mvc.LoggerService.Contracts;

namespace SpyStore.Mvc.Controllers
{
    /// <summary>
    /// The ProductsController needs an instance of the SpyStoreServiceWrapper to
    /// make calls to the SpyStore.Service and an instance of IConfiguration to pass into
    /// the base class for the fake authentication.
    /// </summary>
    [Route("[controller]/[action]")]
    public class ProductsController : BaseController
    {
        private readonly SpyStoreServiceWrapper _serviceWrapper;

        public ProductsController(IConfiguration configuration, ILoggerManager logger, SpyStoreServiceWrapper serviceWrapper)
            : base(configuration, logger)
        {
            _serviceWrapper = serviceWrapper;
        }

        [Route("/")]
        [Route("/Products")]
        [Route("/Products/Index")]
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Featured));
        }

        [HttpGet("{id}")]
        public ActionResult Details(int id)
        {
            return RedirectToAction(nameof(CartController.AddToCart), nameof(CartController).Replace("Controller", ""),
                      new { productId = id, cameFromProducts = true });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductList(int id)
        {
            var category = await _serviceWrapper.GetCategoryAsync(id);

            // begins by setting the necessary ViewBag properties for the View.
            ViewBag.Title = category?.CategoryName;
            ViewBag.Header = category?.CategoryName;
            ViewBag.ShowCategory = false;
            ViewBag.Featured = false;

            IList<ProductViewModel> productViewModels = await _serviceWrapper.GetProductsForACategoryAsync(id);
            return View(productViewModels);

        }

        [HttpGet]
        public async Task<IActionResult> Featured()
        {
            ViewBag.Title = "Featured Products";
            ViewBag.Header = "Featured Products";
            ViewBag.ShowCategory = true;
            ViewBag.Featured = true;

            IList<ProductViewModel> vm = await _serviceWrapper.GetFeaturedProductsAsync();

            return View("ProductList", vm);
        }

        [Route("{searchString}")]
        [HttpPost]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewBag.Title = "Search Results";
            ViewBag.Header = "Search Results";
            ViewBag.ShowCategory = true;
            ViewBag.Featured = false;

            IList<ProductViewModel> vm = await _serviceWrapper.SearchAsync(searchString);

            return View("ProductList", vm);
        }
    }
}