using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SpyStore.Mvc.Support;

namespace SpyStore.Mvc.ViewComponents
{
    public class Menu : ViewComponent
    {
        private readonly SpyStoreServiceWrapper _serviceWrapper;

        public Menu(SpyStoreServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        /// <summary>
        /// When a View Component is rendered from a View, the public method InvokeAsync is invoked.
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cats = await _serviceWrapper.GetCategoriesAsync();

            if (cats == null)
                return new ContentViewComponentResult("There was an error getting the categories");

            return View("MenuView", cats);
        }
    }
}
