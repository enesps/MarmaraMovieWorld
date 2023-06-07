using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Xml.Linq;

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

        public string userId { get; set; }
        public Movie Movie { get; set; }
        public int LikeCount { get; set; }
        public List<Comment> Comments { get; set; }
        public List<CommentViewModel> ViewComments { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Kullan�c�n�n kimli�ini al�n
                userId = await _operationsService.GetCurrentUserId(User);
                Console.WriteLine("USER ID: ", userId);
            }

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
                ViewComments = await _operationsService.GetCommentsForMovie(id);
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
                userId = await _operationsService.GetCurrentUserId(User);
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
            return RedirectToPage("MovieDetail", new { id = movieId });
        }

        public async Task<IActionResult> OnPostComment(int movieId, string commentText)
        {

            Console.WriteLine("onpostcomment");

            if (User.Identity.IsAuthenticated)
            {
                // Kullan�c�n�n kimli�ini al�n
                userId = await _operationsService.GetCurrentUserId(User);

                // Comment ekleme i�lemini ger�ekle�tirin
                await _operationsService.AddComment(userId, movieId, commentText);
            }
            else
            {
                // Kullan�c� giri� yapmam��sa y�nlendirme yap veya giri� yapmas� i�in i�lem yap
                return RedirectToPage("Login");
            }

            return RedirectToPage("MovieDetail", new { id = movieId });
        }

        public async Task<IActionResult> OnPostDeleteComment(int commentId, int movieId)
        {
            Console.WriteLine("delete comment");

            if (User.Identity.IsAuthenticated)
            {
                // Kullan�c�n�n kimli�ini al�n
                userId = await _operationsService.GetCurrentUserId(User);

                // Comment silme i�lemini ger�ekle�tirin
                Console.WriteLine("userId in postdeletecomment: ", userId);
                // Comment ekleme i�lemini ger�ekle�tirin
                await _operationsService.DeleteComment(commentId, userId);
            }
            else
            {
                // Kullan�c� giri� yapmam��sa y�nlendirme yap veya giri� yapmas� i�in i�lem yap
                return RedirectToPage("Login");
            }

            return RedirectToPage("MovieDetail", new { id = movieId });
        }

    }

}
