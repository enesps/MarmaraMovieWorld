using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarmaraMovieWorld.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ILogger<SearchModel> _logger;
        private readonly TMDbService _tmdbService;

        public SearchModel(ILogger<SearchModel> logger, TMDbService tmdbService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
        }

        public SearchResult SearchResult { get; set; }

        public async Task OnGet(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                SearchResult = await _tmdbService.SearchMovies(searchQuery);
            }
        }
    }
}
