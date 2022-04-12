using BlogDAL.Entities;
using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
