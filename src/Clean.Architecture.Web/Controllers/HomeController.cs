using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace Clean.Architecture.Web.Controllers
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
            _logger.LogInformation("Called GET on {action}", nameof(Index));
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
