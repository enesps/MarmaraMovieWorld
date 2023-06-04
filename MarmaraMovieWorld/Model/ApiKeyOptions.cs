using System.Text.Json.Serialization;

namespace MarmaraMovieWorld.Model
{
    public class ApiKeysOptions
    {
        [JsonPropertyName("TMDb")]
        public string TMDb { get; set; }
    }
}
