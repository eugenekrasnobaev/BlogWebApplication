using BlogDAL.Entities;
using BlogDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByName(string name);

        User GetByEmail(string email);
        
        User GetByEmailAndPassword(string email, string password);

        int GetNumberOfUserPosts(int id);

        int GetNumberOfUserComments(int id);
    }
}
