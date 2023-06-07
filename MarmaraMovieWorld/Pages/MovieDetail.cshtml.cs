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
                // Kullanýcýnýn kimliðini alýn
                userId = await _operationsService.GetCurrentUserId(User);
                Console.WriteLine("USER ID: ", userId);
            }

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
                ViewComments = await _operationsService.GetCommentsForMovie(id);
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
                userId = await _operationsService.GetCurrentUserId(User);
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
            return RedirectToPage("MovieDetail", new { id = movieId });
        }

        public async Task<IActionResult> OnPostComment(int movieId, string commentText)
        {

            Console.WriteLine("onpostcomment");

            if (User.Identity.IsAuthenticated)
            {
                // Kullanýcýnýn kimliðini alýn
                userId = await _operationsService.GetCurrentUserId(User);

                // Comment ekleme iþlemini gerçekleþtirin
                await _operationsService.AddComment(userId, movieId, commentText);
            }
            else
            {
                // Kullanýcý giriþ yapmamýþsa yönlendirme yap veya giriþ yapmasý için iþlem yap
                return RedirectToPage("Login");
            }

            return RedirectToPage("MovieDetail", new { id = movieId });
        }

        public async Task<IActionResult> OnPostDeleteComment(int commentId, int movieId)
        {
            Console.WriteLine("delete comment");

            if (User.Identity.IsAuthenticated)
            {
                // Kullanýcýnýn kimliðini alýn
                userId = await _operationsService.GetCurrentUserId(User);

                // Comment silme iþlemini gerçekleþtirin
                Console.WriteLine("userId in postdeletecomment: ", userId);
                // Comment ekleme iþlemini gerçekleþtirin
                await _operationsService.DeleteComment(commentId, userId);
            }
            else
            {
                // Kullanýcý giriþ yapmamýþsa yönlendirme yap veya giriþ yapmasý için iþlem yap
                return RedirectToPage("Login");
            }

            return RedirectToPage("MovieDetail", new { id = movieId });
        }

    }

}
