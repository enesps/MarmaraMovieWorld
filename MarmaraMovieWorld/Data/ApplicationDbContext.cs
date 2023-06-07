using Microsoft.EntityFrameworkCore;
using MarmaraMovieWorld.Model;

namespace MarmaraMovieWorld.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
            
        }
        public DbSet<User> Users { get; set; }
    }
}
