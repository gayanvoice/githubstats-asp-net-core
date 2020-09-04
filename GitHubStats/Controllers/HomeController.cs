using GitHubStats.Models;
using GitHubStats.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GitHubStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CountryService _countryService;
        private readonly GitHubModel _gitHubModel;


        [ViewData]
        public GitHubModel gitHubViewModel { get; set; }

        public HomeController(ILogger<HomeController> logger,
                              CountryService countryService,
                              GitHubModel gitHubModel)
        {
            _logger = logger;
            _countryService = countryService;
            _gitHubModel = gitHubModel;
        }

        public IActionResult Index()
        {
            gitHubViewModel = _gitHubModel;
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
