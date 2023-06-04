using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarmaraMovieWorld.Pages
{ 
    public class SearchResultModel : PageModel
    {
        private readonly ILogger<SearchResultModel> _logger;
        private readonly TMDbService _tmdbService;

        public SearchResultModel(ILogger<SearchResultModel> logger, TMDbService tmdbService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
        }

        public Movie SearchResult { get; set; }

        public async Task<IActionResult> OnGet(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                SearchResult = await _tmdbService.SearchMovies(searchQuery);
            }
          
            return Page();
        }

    }
}