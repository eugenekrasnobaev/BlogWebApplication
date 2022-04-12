using BlogDAL.Entities;
using BlogDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BlogDAL.Repositories
{
    public class PostRepository : BaseRepository<Post>
    {
        public PostRepository(IBlogContext dbContext)
        {
            Db = dbContext;
        }

        public override IEnumerable<Post> GetAll()
        {
            var query = Db.Comments
                .Include(c => c.Post)
                    .ThenInclude(p => p.Author)
                .Include(c => c.Author)
                .Include(c => c.Receiver)
                .Where(c => c.IsActive && c.Post.IsActive);

            var posts = query
                .ToList()
                .Select(c => c.Post)
                .Distinct()
                .Union(Db.Posts
                    .Include(p => p.Author)
                    .Where(p => p.Comments.Count(c => c.IsActive) == 0 && p.IsActive));

            return posts;
        }
    }
}
