namespace MarmaraMovieWorld.Model
{
    public class Movie
    {
        public bool Adult { get; set; }
        public string Backdrop_Path { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
        public int Id { get; set; }
        public string Original_Language { get; set; }
        public string Original_Title { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public string Poster_Path { get; set; }
        public string Release_Date { get; set; }
        public string Title { get; set; }
        public bool Video { get; set; }
        public double Vote_Average { get; set; }
        public int Vote_Count { get; set; }
    }
}
