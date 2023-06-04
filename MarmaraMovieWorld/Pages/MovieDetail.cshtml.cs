using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarmaraMovieWorld.Pages
{
    public class MovieDetailModel : PageModel
    {
        private readonly ILogger<MovieDetailModel> _logger;
        private readonly TMDbService _tmdbService;

        public MovieDetailModel(ILogger<MovieDetailModel> logger, TMDbService tmdbService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
        }

        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id > 0)
            {
                Movie = await _tmdbService.GetMovieDetails(id);
                if (Movie == null)
                {
                    // Film bulunamadý
                    return NotFound();
                }
            }
            else
            {
                // Geçersiz film ID'si
                return NotFound();
            }

            return Page();
        }
    }

}
