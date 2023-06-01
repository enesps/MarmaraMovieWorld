using System.Net;
using System.Text.Json;
using WebProject.Model;
namespace WebProject.Services
{
    public class JSONWikiService
    {
         public WikiModel GetWikiModel(string term)
        {
            string url = string.Concat("https://api.agify.io/?name=", term);
            var json=new WebClient().DownloadString(url);
            WikiModel wikidata=JsonSerializer.Deserialize<WikiModel>(json);
            return wikidata;
        }   
    }
}
