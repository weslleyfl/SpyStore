using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpyStore.Mvc.Models;
using Microsoft.Extensions.Configuration;
using SpyStore.Mvc.Controllers.Base;
using SpyStore.Mvc.LoggerService.Contracts;

namespace SpyStore.Mvc.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : BaseController
    {
               
        public HomeController(IConfiguration configuration, ILoggerManager logger)
            : base(configuration, logger)
        {
            
        }
        
       
        public IActionResult Index()
        {
            //_logger.LogInfo("Fetching all the Students from the storage");
            //_logger.LogDebug("Here is debug message from the controller.");
            //throw new Exception("Exception while fetching all the students from the storage.");
            //_logger.LogWarn("Here is warn message from the controller.");
            //_logger.LogError("Here is error message from the controller.");
            ViewBag.Title = "Home";

            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
