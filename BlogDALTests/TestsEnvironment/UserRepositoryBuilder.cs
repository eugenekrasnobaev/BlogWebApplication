using BlogDAL.Entities;
using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using BlogDALTests.TestsEnvironment;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BlogDALTests.TestsEnvironment
{
    public class UserRepositoryBuilder 
    {
        public DataSetBuilder<User, BlogContext> UserSetBuilder { get; set; }

        private User user;

        public UserRepositoryBuilder()
        {
            UserSetBuilder = new DataSetBuilder<User, BlogContext>();
        }

        public UserRepository Build()
        {
            UserSetBuilder.SetOptions(c => c.Users);
            return new UserRepository(UserSetBuilder.DbContext.Object);
        }

        public UserRepositoryBuilder WithUser(int id, string name, string email, string password)
        {
            user = new User {Id = id, Name = name, Email = email, Password = password};

            UserSetBuilder.SetData(user);
            return this;
        }

        public UserRepositoryBuilder WithPost(int id, string title, string text)
        {
            var post = new Post { Id = id, Title = title, Text = text};
            user?.Posts.Add(post);

            return this;
        }

        public UserRepositoryBuilder WithComment(int id, string text)
        {
            var comment = new Comment { Id = id, Text = text };
            user?.AuthorComments.Add(comment);

            return this;
        }
    }
}
