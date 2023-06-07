using System;
using System.ComponentModel.DataAnnotations;

namespace MarmaraMovieWorld.Model
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int MovieId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
