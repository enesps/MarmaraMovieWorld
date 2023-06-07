using System.ComponentModel.DataAnnotations;

namespace MarmaraMovieWorld.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
