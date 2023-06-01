using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProject.Model;
using WebProject.Services;

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
        WikiModel wikidata { get; set; }
        [BindProperty]
        public string Data { get; set; }
        public void OnPostWikiRequest(JSONWikiService jsonWikiService)
        {
            Console.WriteLine(Term);
            wikidata = jsonWikiService.GetWikiModel(Term);
            Data = wikidata.age.ToString();
        }


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


