using MarmaraMovieWorld.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace MarmaraMovieWorld.Services
{
    public class TMDbService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public TMDbService(IOptions<ApiKeysOptions> options, HttpClient httpClient)
        {
            _apiKey = options.Value.TMDb;
            _httpClient = httpClient;
        }

        public async Task<SearchResult> SearchMovies(string searchQuery)
        {
            string formattedSearchQuery = searchQuery.Replace(" ", "-");
            // String url = $"https://api.themoviedb.org/3/search/movie?api_key=1d2d30d95d1e90b4cf57935aa21669c7&query={searchQuery}";
            string url = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={formattedSearchQuery}";
            Console.WriteLine("Request URL: " + url);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Console.WriteLine("Response status code: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + json);

                // JSON'u çözümleme
                JObject obj = JObject.Parse(json);

                // İlk sonucu alma
                JToken firstResult = obj["results"][0];

                Console.WriteLine("First Result JSON: " + firstResult);

                SearchResult searchResult = firstResult.ToObject<SearchResult>();
                return searchResult;
            }
            else
            {
                Console.WriteLine("Error occurred. Status code: " + response.StatusCode);
                return null;
            }
        }
    }
}
