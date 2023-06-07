using MarmaraMovieWorld.Data;
using MarmaraMovieWorld.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MarmaraMovieWorld.Services
{
    public class OperationsService
    {
        private readonly ApplicationDbContext _dbContext;

        public OperationsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LikeMovie(string userId, int movieId)
        {
            // Kullanıcının filmi daha önce beğenip beğenmediğini kontrol et
            bool isLiked = await _dbContext.Likes.AnyAsync(l => l.UserId == userId && l.MovieId == movieId);

            if (isLiked)
            {
                // Kullanıcı filmi daha önce beğenmiş, like'ı sil
                var existingLike = await _dbContext.Likes.SingleOrDefaultAsync(l => l.UserId == userId && l.MovieId == movieId);
                _dbContext.Likes.Remove(existingLike);
            }
            else
            {
                // Kullanıcı filmi beğenmemiş, like ekle
                var like = new Like
                {
                    UserId = userId,
                    MovieId = movieId
                };
                _dbContext.Likes.Add(like);
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task CommentMovie(string userId, int movieId, string commentText)
        {
            var comment = new Comment
            {
                UserId = userId,
                MovieId = movieId,
                Content = commentText,
                CreatedAt = DateTime.Now
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetCurrentUserId(ClaimsPrincipal user)
        {
            // Kullanıcının e-posta adresini al
            var email = user.FindFirst(ClaimTypes.Email)?.Value;

            if (email != null)
            {
                // E-postaya karşılık gelen kullanıcıyı veritabanından bul
                var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (userFromDb != null)
                {
                    // Kullanıcının UserId'sini döndür
                    return userFromDb.Id.ToString();
                }
            }

            // Kullanıcı veya kullanıcının UserId'si bulunamadıysa null döndür
            return null;
        }

        public async Task<int> GetLikeCountForMovie(int movieId)
        {
            return await _dbContext.Likes.CountAsync(l => l.MovieId == movieId);
        }

        public async Task AddComment(string userId, int movieId, string commentText)
        {
            var comment = new Comment
            {
                UserId = userId,
                MovieId = movieId,
                Text = commentText,
                CreatedAt = DateTime.Now
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }

    }

}

