using System;
using System.ComponentModel.DataAnnotations;

namespace MarmaraMovieWorld.Model
{    public class CommentViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
