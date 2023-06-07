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
                    TempData["StatusMessage"] = "Movie couldn't be found:(";
                    Console.Write("MESSAGE\n");
                    return RedirectToPage("Index");
                }
            }

            PopularMovies = await _tmdbService.GetPopularMovies();

            return Page();
        }

    }
}
