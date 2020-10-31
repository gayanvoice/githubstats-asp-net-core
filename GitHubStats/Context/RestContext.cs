using GitHubStats.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHubStats.Context
{
    public class RestContext
    {
        public async Task<GraphQLModel> RequestGitHubAuthorizationKeys()
        {
            var graphQL = createClient().GetStreamAsync("https://githubstatsfun.azurewebsites.net/api/requestGitHubAuthorizationKeys?code=p7Gw3OW7Zqk2kXFguIwWhyrjR6j5vHioP/r5igh8O4ygQsoIkyB8cw==");
            var repositories = await JsonSerializer.DeserializeAsync<GraphQLModel>(await graphQL);
            return repositories;
        }

        private HttpClient createClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            return client;
        }
    }
}