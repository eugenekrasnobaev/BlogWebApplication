using System;
using System.Collections.Generic;
using System.Linq;
using BlogBLL.DTO;
using BlogDAL.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlogBLLTests.Services
{
    [TestClass]
    public class BlogServiceTests
    {
        [TestMethod]
        public void TestCreatePost()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            var user = new User{ Id = 1, Email = "user@user", Name = "Ivan", Password = "11111" };
            var createPostDto = new CreatePostDto { Id = 1, Title = "Hi!", Text = "Hello!" };

            builder.UserRepository.Setup(r => r.GetByName("Ivan")).Returns(user);

            service.CreatePost(createPostDto, "Ivan");

            builder.PostRepository.Verify(r => r.Add(It.IsAny<Post>()));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreatePost_WhenUserDoesNotExists_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.CreatePost(new CreatePostDto(), "user@user");
        }
        
        [TestMethod]
        public void TestPostsList()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var posts = new List<Post>
            {
                new Post{ Id = 1, Title = "Hi!", Text = "Hello!"},
                new Post{ Id = 2, Title = "ST", Text = "Some text"}
            };

            builder.PostRepository.Setup(r => r.GetAll()).Returns(posts);

            var actual = service.PostsList().ToList();

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual("Hi!", actual.Single(p => p.Id == 1).Title );
        }
        
        [TestMethod]
        public void TestPostsList_WhenNoPosts_ShouldReturnEmptyEnumerable()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            var actual = service.PostsList();

            Assert.AreEqual(0, actual.Count());
        }
        
        [TestMethod]
        public void TestGetPostById_WhenPostExists()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var post = new Post { Id = 1, Title = "Hi!", Text = "Hello!"};

            builder.PostRepository.Setup(r => r.GetById(1)).Returns(post);

            var actual = service.GetPostById(1);

            Assert.AreEqual("Hi!", actual.Title);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetPostById_WhenNoPosts_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.GetPostById(1);
        }
        
        [TestMethod]
        public void TestDeletePost_WhenPostExists()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var post = new Post { Id = 1, Title = "Hi!", Text = "Hello!"};
            var comment = new Comment { Id = 1, Text = "Hello!", PostId = 1};
            post.Comments.Add(comment);
            
            builder.PostRepository.Setup(r => r.GetByIdWithInclude(1,  p => p.Comments))
                .Returns(post);

            service.DeletePost(1);

            builder.PostRepository.Verify(r => r.Delete(post));
            builder.CommentRepository.Verify(r => r.Delete(comment));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeletePost_WhenNoPosts_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.DeletePost(1);
        }
        
        [TestMethod]
        public void TestEditPost_WhenPostExists()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var post = new Post { Id = 1, Title = "Hi!", Text = "Hello!" };
            builder.PostRepository.Setup(r => r.GetById(1)).Returns(post);

            service.EditPost(new EditPostDto{ Text = "Hello!!!" }, 1);

            Assert.AreEqual("Hello!!!", post.Text);
            builder.PostRepository.Verify(r => r.Update(post));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEditPost_WhenNoPosts_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.EditPost(new EditPostDto(), 1);
        }
        
        [TestMethod]
        public void TestCreateComment()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            var user = new User { Id = 1, Name = "Ivan", Email = "user@user", Password = "11111" };
            var admin = new User { Id = 1, Name = "Frank", Email = "admin@admin", Password = "11111" };
            var createCommentDto = new CreateCommentDto { Id = 1, Text = "Hello!" };

            builder.UserRepository.Setup(r => r.GetByName("Ivan")).Returns(user);
            builder.UserRepository.Setup(r => r.GetByName("Frank")).Returns(admin);

            service.CreateComment(createCommentDto, "Ivan", 1, "Frank");

            builder.CommentRepository.Verify(r => r.Add(It.IsAny<Comment>()));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateComment_WhenUserDoesNotExists_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.CreateComment(new CreateCommentDto(), "user@user", 1, "admin@admin");
        }
        
        [TestMethod]
        public void TestGetCommentById_WhenCommentExists()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var comment = new Comment { Id = 1, Text = "Hello!" };

            builder.CommentRepository.Setup(r => r.GetById(1)).Returns(comment);

            var actual = service.GetCommentById(1);

            Assert.AreEqual("Hello!", actual.Text);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetCommentById_WhenNoComments_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.GetCommentById(1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteComment_WhenCommentExists_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var comment = new Comment { Id = 1, Text = "Hello!" };
            builder.CommentRepository.Setup(r => r.GetByIdWithInclude(1))
                .Returns(comment);

            service.DeletePost(1);

            builder.CommentRepository.Verify(r => r.Delete(comment));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteComment_WhenNoComment_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.DeleteComment(1);
        }
        
        [TestMethod]
        public void TestEditComment_WhenCommentExists()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var comment = new Comment { Id = 1, Text = "Hello!" };
            builder.CommentRepository.Setup(r => r.GetById(1)).Returns(comment);

            service.EditComment(new EditCommentDto { Text = "Hello!!!" }, 1);

            Assert.AreEqual("Hello!!!", comment.Text);
            builder.CommentRepository.Verify(r => r.Update(comment));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEditComment_WhenNoComments_ShouldExpectException()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();

            service.EditComment(new EditCommentDto { Text = "Hello!!!" }, 1);
        }

        [TestMethod]
        public void TestGetUserInfo_WhenUserExists()
        {
            var builder = new BlogServiceBuilder();
            var service = builder.Create();
            var user = new User { Id = 1, Email = "user@user", Password = "11111" };

            builder.UserRepository.Setup(r => r.GetById(1)).Returns(user);
            builder.UserRepository.Setup(r => r.GetNumberOfUserPosts(1))
                .Returns(1);
            builder.UserRepository.Setup(r => r.GetNumberOfUserComments(1))
                .Returns(1);

            var actual = service.GetUserInfo(1);

            Assert.AreEqual("user@user", actual.User.Email);
            Assert.AreEqual(1, actual.NumberOfUserPosts);
            Assert.AreEqual(1, actual.NumberOfUserComments);
        }
    }
}
