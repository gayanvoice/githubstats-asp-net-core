using GitHubStats.Models;
using GitHubStats.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Collections.Generic;

namespace GitHubStats.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly CountryService _countryService;
        private readonly ILogger<ApiController> _logger;
        private readonly GitHubModel _gitHubModel;
        public ApiController(CountryService userService,
                             ILogger<ApiController> logger,
                             GitHubModel gitHubModel)
        {
            _countryService = userService;
            _logger = logger;
            _gitHubModel = gitHubModel;
        }


        [HttpGet("country/list")]
        public ActionResult<List<GitHubModel.CountryModel>> GetCountryList()
        {
            return _gitHubModel.Country;
        }

        [HttpGet("country/{countryName}/{limit}")]
        public ActionResult<List<UserBsonModel>> GetUsersByLocation(string countryName, int limit)
        {
            FindUserRequestModel findUserRequestModel = new FindUserRequestModel();
            findUserRequestModel.CountryName = countryName;
            findUserRequestModel.Limit = limit;
            var userList =_countryService.GetUserListByCountry(findUserRequestModel);
            if (userList.Count == 0)
            {
                _logger.LogInformation("no users");
                return NoContent();
            }
            else
            {
                _logger.LogInformation("available users");
                return userList;
            }
        }
    }
}