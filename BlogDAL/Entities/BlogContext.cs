using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

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
