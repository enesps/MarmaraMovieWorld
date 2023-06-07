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
                Content = commentText,
                CreatedAt = DateTime.Now
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CommentViewModel>> GetCommentsForMovie(int movieId)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.MovieId == movieId)
                .ToListAsync();

            var commentViewModels = new List<CommentViewModel>();
            foreach (var comment in comments)
            {
                var Name = await GetName(comment.UserId);

                var commentViewModel = new CommentViewModel
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    Content = comment.Content,
                    Name = Name,
                    CreatedAt = comment.CreatedAt
                };

                commentViewModels.Add(commentViewModel);
            }

            return commentViewModels;
        }

        public async Task<string> GetName(string userId)
        {
            // Kullanıcı adını almak için ilgili veritabanı sorgularını yapın
            // Örnek olarak, EF Core kullanarak Users tablosundan kullanıcı adını alabilirsiniz
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            Console.WriteLine(user?.Name);
            // Kullanıcı adını döndürün
            return user?.Name;
        }

        public async Task DeleteComment(int commentId, string userId)
        {
            Console.WriteLine("ddddelete comment");
            var comment = await _dbContext.Comments.FindAsync(commentId);
            // Console.WriteLine(comment.UserId , " " , userId );
            if (comment != null && comment.UserId == userId)
            {
                Console.WriteLine("IF delete comment");
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
            }
        }


    }

}

