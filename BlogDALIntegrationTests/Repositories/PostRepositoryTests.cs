using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogDAL.Repositories;
using System.Linq;
using BlogDAL.Interfaces;
using BlogDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogDALIntegrationTests.Repositories
{
    [TestClass]
    public class PostRepositoryTests
    {
        private PostRepository postRepository;
        private IUserRepository userRepository;
        private BlogContext context;

        private User user;

        [TestInitialize]
        public void Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();

            var options = optionsBuilder
                .UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = blogdb; Trusted_Connection = True;")
                .Options;

            context = new BlogContext(options);
            postRepository = new PostRepository(context);
            userRepository = new UserRepository(context);

            user = new User { Email = "test@test", Password = "12345" };
            userRepository.Add(user);
            userRepository.Save();
        }

        [TestCleanup]
        public void Cleanup()
        {
            userRepository.Delete(user);
            userRepository.Save();

            context.Dispose();
        }

        public Post TestAdd()
        {
            var post = new Post {Title = "Hi!", Text = "Hello!", AuthorId = user.Id};
            postRepository.Add(post);
            postRepository.Save();

            var actual = postRepository.GetById(post.Id);

            Assert.AreEqual("Hi!", actual.Title, "Create fail");

            return post;
        }

        public void TestGetById(Post post)
        {
            var actual = postRepository.GetById(post.Id);

            Assert.AreEqual("Hi!", actual.Title, "Get fail");
        }

        public void TestGetAll()
        {
            var posts = postRepository.GetAll();

            Assert.IsTrue(posts.Any(), "GetAll fail");
            Assert.IsTrue(posts.First().Comments.Any(), "GetAll fail");
            Assert.IsNotNull(posts.First().Author, "GetAll fail");
        }

        public void TestDelete(Post post)
        {
            postRepository.Delete(post);
            postRepository.Save();

            var actual = postRepository.GetById(post.Id);

            Assert.AreEqual(null, actual, "Delete fail");
        }

        public void TestUpdate(Post post, string newText)
        {
            post.Text = newText;
            postRepository.Update(post);
            postRepository.Save();

            var actual = postRepository.GetById(post.Id);

            Assert.AreEqual(newText, actual.Text, "Update fail");
        }

        [TestMethod]
        public void TestScript()
        {
            var post = TestAdd();
            TestGetById(post);
            TestGetAll();
            TestUpdate(post, "Text");
            TestDelete(post);
        }
    }
}