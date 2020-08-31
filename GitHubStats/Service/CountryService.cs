using GitHubStats.Context;
using GitHubStats.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace GitHubStats.Service
{
    public class CountryService
    {
        private IMongoCollection<CountryModel> collection;

        public CountryService(MongoModel mongoModel)
        {
            collection = new MongoContext(mongoModel)
                .database
                .GetCollection<CountryModel>(mongoModel.Collection.User);
        }
        public List<CountryModel> GetList()
        {
            return collection.Find(country => true).ToList();
        }

        public CountryModel GetCountry(string countryName)
        {
            return collection.Find<CountryModel>(country => country.CountryName == countryName).FirstOrDefault();
        }

        public CountryModel CreateCountry(CountryModel countryModel)
        {
            collection.InsertOne(countryModel);
            return countryModel;
        }

        public void UpdateCountry(string countryName, CountryModel countryModel)
        {
            collection.ReplaceOne(country => country.CountryName == countryName, countryModel);
        }
    }
}