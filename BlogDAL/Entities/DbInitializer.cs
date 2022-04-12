using BlogDAL.Interfaces;
using System.Linq;

namespace BlogDAL.Entities
{
    public static class DbInitializer
    {
        public static void Seed(BlogContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any() || context.Posts.Any() || context.Comments.Any())
            {
                return;
            }

            AddContent(context);
        }

        private static void AddContent(IBlogContext context)
        {
            var user = new User { Name = "Ivan", Email = "user@user", Password = "12345", IsExternalAccount = false, Role = Role.User};
            var admin = new User { Name = "Frank", Email = "admin@admin", Password = "12345", IsExternalAccount = false, Role = Role.Admin };
            context.Users.AddRange(user, admin);

            var post = new Post { Title = "Hello!", Text = "Hello world!", Author = user };
            context.Posts.Add(post);

            var comment = new Comment { Text = "Need proofs!", Post = post, Author = user, Receiver = null};
            var subComment = new Comment { Text = "Yes!", Post = post, Author = admin, Receiver = user };
            context.Comments.AddRange(comment, subComment);

            context.SaveChanges();
        }
    }
}
