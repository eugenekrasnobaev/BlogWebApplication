using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlogDAL.Entities;
using BlogDAL.Repositories;
using BlogDALTests.RepositoryTests;
using BlogDALTests.TestsEnvironment;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlogDALTests.RepositoryTests
{
    [TestClass]
    public class PostRepositoryTests
    {
        [TestMethod]
        public void TestAdd()
        {
            var repository = new PostRepositoryBuilder().Build();

            repository.Add(new Post { Id = 1, Title = "Hello!", Text = "Hello, world!" });

            Assert.AreEqual("Hello!", repository.GetById(1).Title);
        }

        [TestMethod]
        public void TestGetById_WhenPostExist_ShouldReturnPost()
        {
            var repository = new PostRepositoryBuilder()
                .WithPost(1, "Hello!", "Hello world!")
                .Build();

            var actual = repository.GetById(1);

            Assert.AreEqual("Hello!", actual.Title);
        }

        [TestMethod]
        public void TestGetById_WhenPostDoesNotExist_ShouldReturnNull()
        {
            var repository = new PostRepositoryBuilder()
                .Build();

            var actual = repository.GetById(1);

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetById_WhenPostNonExist_ShouldReturnNull()
        {
            var repository = new PostRepositoryBuilder().Build();

            var actual = repository.GetById(2);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetAll_ShouldReturnEntities()
        {
            var postBuilder = new PostRepositoryBuilder();
            var postRepository = postBuilder
                .WithPost(1, "Hello!", "Hello world!")
                .WithPost(2, "Hi!", "How are you?")
                .Build();

            var actual = postRepository.GetAll().ToList();

            Assert.IsTrue(actual.SequenceEqual(postBuilder.PostSetBuilder.Data));
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        public void TestDelete_WhenPostExist()
        {
            var builder = new PostRepositoryBuilder();
            var repository = builder
                .WithPost(1, "Hello!", "Hello world!")
                .Build();
            var post = builder.PostSetBuilder.GetData(1);

            repository.Delete(post);

            builder.DbContext.Verify(c => c.Update(post));
            Assert.IsFalse(post.IsActive);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestDelete_WhenPostNonExist_ShouldExpectException()
        {
            var repository = new PostRepositoryBuilder().Build();

            repository.Delete(new Post { Id = 1, Title = "Hello!", Text = "Hello, world!" });
        }

        [TestMethod]
        public void TestDeleteFromDb_WhenUserExist()
        {
            var builder = new PostRepositoryBuilder();
            var repository = builder
                .WithPost(1, "Hello!", "Hello world!")
                .Build();
            var post = builder.PostSetBuilder.GetData(1);

            repository.DeleteFromDb(post);

            builder.DbContext.Verify(c => c.Remove(post));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestDeleteFromDb_WhenUserExist_ShouldExpectException()
        {
            var builder = new PostRepositoryBuilder();
            var repository = builder.Build();

            repository.DeleteFromDb(new Post { Id = 1, Title = "Hello!", Text = "Hello, world!" });
        }

        [TestMethod]
        public void TestGet_ById_ShouldReturnEntities()
        {
            var repository = new PostRepositoryBuilder()
                .WithPost(1, "Hello!", "Hello world!")
                .Build();

            var actual = repository.Get(u => u.Id == 1);

            Assert.AreEqual(1, actual.Count());
        }

        [TestMethod]
        public void TestGet_ByTitle_ShouldReturnEntities()
        {
            var repository = new PostRepositoryBuilder()
                .WithPost(1, "Hello!", "Hello world!")
                .Build();

            var actual = repository.Get(u => u.Title == "Hello!");

            Assert.AreEqual(1, actual.Count());
        }

        [TestMethod]
        public void TestGet_ById_WhenPostNonExist_ShouldReturnEmptyEnumerable()
        {
            var repository = new PostRepositoryBuilder()
                .WithPost(1, "Hello!", "Hello world!")
                .Build();

            var actual = repository.Get(u => u.Id == 100);

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestUpdate_WhenPostExist()
        {
            var builder = new PostRepositoryBuilder();
            var repository = builder
                .WithPost(1, "Hello!", "Hello world!")
                .WithPost(2, "Hi!", "How are you?")
                .Build();
            var post = builder.PostSetBuilder.GetData(1);

            repository.Update(post);

            builder.PostSetBuilder.DbContext.Verify(c => c.Update(post));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestUpdate_WhenUserNonExist_ShouldExpectException()
        {
            var repository = new PostRepositoryBuilder().Build();

            repository.Update(new Post { Id = 1, Title = "Hello!", Text = "Hello, world!" });
        }
    }
}