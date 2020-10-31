using System.Collections.Generic;
using System.Diagnostics;
using GitHubStats.Models;
using GitHubStats.Service;
using Microsoft.AspNetCore.Mvc;

namespace GitHubStats.Controllers
{
    public class CountryController : Controller
    {

        private readonly CountryService _countryService;

        public CountryController(CountryService userService)
        {
            _countryService = userService;
        }

        public IActionResult Index(string name, int limit)
        {           
            FindUserRequestModel findUserRequestModel = new FindUserRequestModel();
            findUserRequestModel.CountryName = name;
            findUserRequestModel.Limit = limit;
            ViewData["name"] = name;
            ViewData["numberOfPages"] = _countryService.GetNumberOfUsersByCountry(findUserRequestModel)/limit;
            var userList = _countryService.GetUserListByCountry(findUserRequestModel);
            if (userList.Count == 0)
            {
                ViewData["userList"] = null;
            }
            else
            {
                ViewData["userList"] = userList;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
