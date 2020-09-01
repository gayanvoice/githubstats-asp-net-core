using GitHubStats.Models;
using GitHubStats.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GitHubStats.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly CountryService _countryService;
        private readonly GraphQLService _graphQLService;

        public ApiController(CountryService userService,
                             GraphQLService graphQLService)
        {
            _countryService = userService;
            _graphQLService = graphQLService;
        }

        [HttpGet("country/getList")]
        public ActionResult<List<CountryModel>> GetCountryList()
        {
            return _countryService.GetList();
        }

        [HttpGet("country/getCountry/{countryName}", Name = "GetCountry")]
        public ActionResult<CountryModel> GetCountry(string countryName)
        {
            CountryModel country = _countryService.GetCountry(countryName);
            if (country == null)
            {
                return NotFound();
            }
            else
            {
                return country;
            }
        }

        [HttpPost("country/createCountry")]
        public ActionResult<CountryModel> CreateCountry(CountryModel countryModel)
        {
            _countryService.CreateCountry(countryModel);
            return CreatedAtRoute("GetCountry", new {country = countryModel.CountryName}, countryModel);
        }

        [HttpPut("country/updateCountry/{countryName}")]
        public IActionResult UpdateUser(string countryName, CountryModel countryModel)
        {
            CountryModel country = _countryService.GetCountry(countryName);
            if (country == null)
            {
                return NotFound();
            }
            else
            {
                _countryService.UpdateCountry(countryName, countryModel);
                return NoContent();
            }
        }

        //[HttpGet("country/getGraph")]
        //public ActionResult<string> GetGraph()
        //{
            //GraphQLRequestModel graphQLRequestModel = new GraphQLRequestModel("india", 10, null);
            //var graphQLResponse =  _graphQLService.GetGraphQLHttpResponse(graphQLRequestModel);
            //try
         //   {
           //     return JsonSerializer
             //   .Serialize(graphQLResponse, new JsonSerializerOptions { WriteIndented = true });
        //    }
           // catch (Exception) { 
         //       return BadRequest();
          //  }
       // }
    }
}