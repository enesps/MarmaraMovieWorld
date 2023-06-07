using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarmaraMovieWorld.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly TMDbService _tmdbService;

        public IndexModel(ILogger<IndexModel> logger, TMDbService tmdbService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
        }

        public Movie SearchResult { get; set; }
        public string ErrorMessage { get; set; }
        public List<Movie> PopularMovies { get; set; }
        public async Task OnGet()
        {
            PopularMovies = await _tmdbService.GetPopularMovies();
        }

        public async Task<IActionResult> OnPostAsync(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                SearchResult = await _tmdbService.SearchMovies(searchQuery);

                if (SearchResult != null && SearchResult.Id > 0)
                {
                    return RedirectToPage("MovieDetail", new { id = SearchResult.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Movie couldn't be found.";

                    PopularMovies = await _tmdbService.GetPopularMovies();
                    return Page();
                }
            }

            PopularMovies = await _tmdbService.GetPopularMovies();

            if (TempData.ContainsKey("ErrorMessage"))
            {
                ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
            }
            return Page();

        }

    }
}
