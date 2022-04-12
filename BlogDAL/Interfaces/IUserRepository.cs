using BlogDAL.Entities;

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
