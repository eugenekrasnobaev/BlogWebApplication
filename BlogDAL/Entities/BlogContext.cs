using BlogDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogDAL.Entities
{
    public class BlogContext : DbContext, IBlogContext
    {
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public BlogContext() { }

        public BlogContext(DbContextOptions options) : base(options) { }
    }
}
