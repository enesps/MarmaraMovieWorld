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
                    // Film bulunamadý
                    return NotFound();
                }

                // Beðeni sayýsýný al
                LikeCount = await _operationsService.GetLikeCountForMovie(id);
            }
            else
            {
                // Geçersiz film ID'si
                return NotFound();
            }

            return Page();
        }


        public async Task<IActionResult> OnPostLike(int movieId)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Kullanýcý giriþ yapmýþsa iþlemleri gerçekleþtir
                var userId = await _operationsService.GetCurrentUserId(User);
                await _operationsService.LikeMovie(userId, movieId);

                // Kullanýcýnýn bilgilerini konsola yazdýr
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
            }
            else
            {
                // Kullanýcý giriþ yapmamýþsa yönlendirme yap veya giriþ yapmasý için iþlem yap
                return RedirectToPage("Login");
            }

            // Yönlendirme veya diðer iþlemler
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostComment(int movieId, string commentText)
        {
            // Kullanýcýnýn kimliðini alýn
            var userId = await _operationsService.GetCurrentUserId(User);

            // Comment ekleme iþlemini gerçekleþtirin
            await _operationsService.AddComment(userId, movieId, commentText);

            // Yönlendirme veya diðer iþlemler
            return RedirectToPage("MovieDetail", new { id = movieId });
        }


    }

}
