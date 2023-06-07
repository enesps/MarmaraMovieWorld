using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MarmaraMovieWorld.Pages
{
    public class MovieDetailModel : PageModel
    {
        private readonly ILogger<MovieDetailModel> _logger;
        private readonly TMDbService _tmdbService;
        private readonly OperationsService _operationsService;

        public MovieDetailModel(ILogger<MovieDetailModel> logger, TMDbService tmdbService, OperationsService operationsService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
            _operationsService = operationsService;
        }

        public Movie Movie { get; set; }
        public int LikeCount { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id > 0)
            {
                Movie = await _tmdbService.GetMovieDetails(id);
                if (Movie == null)
                {
                    // Film bulunamad�
                    return NotFound();
                }

                // Be�eni say�s�n� al
                LikeCount = await _operationsService.GetLikeCountForMovie(id);
            }
            else
            {
                // Ge�ersiz film ID'si
                return NotFound();
            }

            return Page();
        }


        public async Task<IActionResult> OnPostLike(int movieId)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Kullan�c� giri� yapm��sa i�lemleri ger�ekle�tir
                var userId = await _operationsService.GetCurrentUserId(User);
                await _operationsService.LikeMovie(userId, movieId);

                // Kullan�c�n�n bilgilerini konsola yazd�r
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
            }
            else
            {
                // Kullan�c� giri� yapmam��sa y�nlendirme yap veya giri� yapmas� i�in i�lem yap
                return RedirectToPage("Login");
            }

            // Y�nlendirme veya di�er i�lemler
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostComment(int movieId, string commentText)
        {
            // Kullan�c�n�n kimli�ini al�n
            var userId = await _operationsService.GetCurrentUserId(User);

            // Comment ekleme i�lemini ger�ekle�tirin
            await _operationsService.AddComment(userId, movieId, commentText);

            // Y�nlendirme veya di�er i�lemler
            return RedirectToPage("MovieDetail", new { id = movieId });
        }


    }

}
