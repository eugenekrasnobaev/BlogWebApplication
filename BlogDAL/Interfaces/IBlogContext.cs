using Microsoft.EntityFrameworkCore;
using BlogDAL.Entities;

namespace BlogDAL.Interfaces
{
    public interface IBlogContext : IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Comment> Comments { get; set; }
    }
}
