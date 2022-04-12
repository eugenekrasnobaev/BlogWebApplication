using System;
using System.Collections.Generic;
using System.Linq;
using BlogDAL.Entities;
using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlogDALTests.Repositories
{
    [TestClass]
    public class PostRepositoryTests
    {
        [TestMethod]
        public void TestCreate()
        {
            var repository = new PostRepositoryBuilder().Build();

            repository.Create(new Post { Id = 1, Title = "Hello!", Text = "Hello, world!" });

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
        public void TestGetById_WhenPostNonExist_ShouldReturnNull()
        {
            var repository = new PostRepositoryBuilder().Build();

            var actual = repository.GetById(2);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetAll_ShouldReturnEntities()
        {
            var builder = new PostRepositoryBuilder();
            var repository = builder
                .WithPost(1, "Hello!", "Hello world!")
                .WithPost(2, "Hi!", "How are you?")
                .Build();

            var actual = repository.GetAll();

            Assert.IsTrue(actual.SequenceEqual(builder.Data));
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        public void TestDelete_WhenPostExist()
        {
            var builder = new PostRepositoryBuilder();
            var repository = builder
                .WithPost(1, "Hello!", "Hello world!")
                .Build();

            repository.Delete(builder.GetEntity(1));

            builder.DbContext.Verify(m => m.Remove(It.IsAny<Post>()));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestDelete_WhenPostNonExist_ShouldReturnException()
        {
            var repository = new PostRepositoryBuilder().Build();

            repository.Delete(new Post { Id = 1, Title = "Hello!", Text = "Hello, world!" });
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

            repository.Update(builder.GetEntity(1));

            builder.DbContext.Verify(m => m.Update(It.IsAny<Post>()));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestUpdate_WhenUserNonExist_ShouldReturnException()
        {
            var repository = new PostRepositoryBuilder().Build();

            repository.Update(new Post { Id = 1, Title = "Hello!", Text = "Hello, world!" });
        }

    }
}