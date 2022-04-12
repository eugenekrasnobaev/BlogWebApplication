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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IBlogContext dbContext)
        {
            Db = dbContext;
        }

        public override User GetById(int id)
        {
            return Db.Users
                .Where(i => i.IsActive)
                .Include(u => u.AuthorComments)
                .Include(u => u.CommentsToAuthor)
                .FirstOrDefault(u => u.Id == id);
        }

        public User GetByEmail(string email)
        {
            return Db.Users
                .Where(i => i.IsActive)
                .FirstOrDefault(u => u.Email == email);
        }

        public User GetByName(string name)
        {
            return Db.Users
                .Where(i => i.IsActive)
                .FirstOrDefault(u => u.Name == name);
        }

        public User GetByEmailAndPassword(string email, string password)
        {
            return Db.Users
                .Where(i => i.IsActive)
                .FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public int GetNumberOfUserPosts(int id)
        {
            return Db.Users.Where(u => u.IsActive && u.Id == id).SelectMany(x => x.Posts).Count(post => post.IsActive);
        }

        public int GetNumberOfUserComments(int id)
        {
            return Db.Users.Where(u => u.IsActive && u.Id == id).SelectMany(x => x.AuthorComments).Count(c => c.IsActive);
            /*
            return Db.Users
                .Include(u => u.AuthorComments)
                .Single(u => u.IsActive && u.Id == id)
                .AuthorComments
                .Count(c => c.IsActive);
           */
        }
    }
}
