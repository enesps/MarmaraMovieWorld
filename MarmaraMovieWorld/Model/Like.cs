using System.ComponentModel.DataAnnotations;

namespace MarmaraMovieWorld.Model
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int MovieId { get; set; }
    }
}
