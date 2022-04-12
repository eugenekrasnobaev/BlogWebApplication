using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogDAL.Repositories;
using System.Linq;
using BlogDAL.Interfaces;
using BlogDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogDALIntegrationTests.Repositories
{
    [TestClass]
    public class CommentRepositoryTests
    {
        private CommentRepository commentRepository;
        private PostRepository postRepository;
        private IUserRepository userRepository;
        private BlogContext context;

        private Post post;
        private User user;

        [TestInitialize]
        public void Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();

            var options = optionsBuilder
                .UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = blogdb; Trusted_Connection = True;")
                .Options;

            context = new BlogContext(options);
            commentRepository = new CommentRepository(context);
            postRepository = new PostRepository(context);
            userRepository = new UserRepository(context);

            user = new User { Email = "test@test", Password = "12345" };
            userRepository.Add(user);
            userRepository.Save();

            post = new Post { Title = "Hi!", Text = "Hello!", AuthorId = user.Id };
            postRepository.Add(post);
            postRepository.Save();
        }

        [TestCleanup]
        public void Cleanup()
        {
            postRepository.Delete(post);
            userRepository.Delete(user);
            userRepository.Save();

            context.Dispose();
        }

        public Comment TestAdd()
        {
            var comment = new Comment { Text = "Fine!", PostId = this.post.Id, AuthorId = user.Id };
            commentRepository.Add(comment);
            commentRepository.Save();

            var actual = commentRepository.GetById(comment.Id);

            Assert.AreEqual("Fine!", actual.Text, "Create fail");

            return comment;
        }

        public void TestGetById(Comment comment)
        {
            var actual = commentRepository.GetById(comment.Id);

            Assert.AreEqual("Fine!", actual.Text, "Get fail");
        }

        public void TestGetAll()
        {
            var comments = commentRepository.GetAll();

            Assert.IsTrue(comments.Any(), "GetAll fail");
        }

        public void TestDelete(Comment comment)
        {
            commentRepository.Delete(comment);
            commentRepository.Save();

            var actual = commentRepository.GetById(comment.Id);

            Assert.AreEqual(null, actual, "Delete fail");
        }

        public void TestUpdate(Comment comment, string newText)
        {
            comment.Text = newText;
            commentRepository.Update(comment);
            commentRepository.Save();

            var actual = commentRepository.GetById(comment.Id);

            Assert.AreEqual(newText, actual.Text, "Update fail");
        }

        [TestMethod]
        public void TestScript()
        {
            var comment = TestAdd();
            TestGetById(comment);
            TestGetAll();
            TestUpdate(comment, "Text");
            TestDelete(comment);
        }
    }
}