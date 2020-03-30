using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using SpyStore.Mvc.LoggerService.Contracts;

namespace SpyStore.Mvc.Controllers.Base
{
    public class BaseController : Controller
    {
        protected readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration, ILoggerManager logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.CustomerId = _configuration.GetValue<int>("CustomerId");
        }
    }
}
