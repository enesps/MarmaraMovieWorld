using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarmaraMovieWorld.Model;
using MarmaraMovieWorld.Services;

namespace MarmaraMovieWorld.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public string Term { get; set; }
     

        [BindProperty]
        public string PokeName { get; set; }
        [BindProperty]
        public string photoPath { get; set; }
        public void OnGet()
        {

        }
        public void OnPostPokeRequest()
        {
            Console.WriteLine(PokeName);
            photoPath = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/" + PokeName + ".png";
        }

    }
}


