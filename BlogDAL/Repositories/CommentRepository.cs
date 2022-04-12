using BlogDAL.Entities;
using BlogDAL.Interfaces;

namespace BlogDAL.Repositories
{
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository(IBlogContext dbContext)
        {
            Db = dbContext;
        }
    }
}
