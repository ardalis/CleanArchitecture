using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace CleanArchitecture.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation($"Called GET on {nameof(Index)}"); //, nameof(Index));
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
