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

        public async Task<Movie> SearchMovies(string searchQuery)
        {
            string formattedSearchQuery = searchQuery.Replace(" ", "-");
            string url = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={formattedSearchQuery}";
            Console.WriteLine("Request URL: " + url);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Console.WriteLine("Response status code: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + json);

                JObject obj = JObject.Parse(json);

                JArray results = obj["results"] as JArray;

                if (results != null && results.Count > 0)
                {
                    JToken firstResult = results[0];

                    Console.WriteLine("First Result JSON: " + firstResult);

                    Movie searchResult = firstResult.ToObject<Movie>();
                    return searchResult;
                }
                else
                {
                    Console.WriteLine("No results found.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Error occurred. Status code: " + response.StatusCode);
                return null;
            }
        }

        public async Task<List<Movie>> GetPopularMovies()
        {
            string url = $"https://api.themoviedb.org/3/movie/popular?api_key={_apiKey}&language=en-US&page=1";
            Console.WriteLine("Request URL: " + url);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Console.WriteLine("Response status code: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + json);

                JObject obj = JObject.Parse(json);

                JArray results = obj["results"] as JArray;

                List<Movie> movies = results.ToObject<List<Movie>>();
                return movies;
            }
            else
            {
                Console.WriteLine("Error occurred. Status code: " + response.StatusCode);
                return null;
            }
        }

        public async Task<Movie> GetMovieDetails(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={_apiKey}";
            Console.WriteLine("Request URL: " + url);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            Console.WriteLine("Response status code: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + json);

                // JSON'u çözümleme
                JObject obj = JObject.Parse(json);

                // Film nesnesini oluşturma
                Movie movie = obj.ToObject<Movie>();
                return movie;
            }
            else
            {
                Console.WriteLine("Error occurred. Status code: " + response.StatusCode);
                return null;
            }
        }

    }
}