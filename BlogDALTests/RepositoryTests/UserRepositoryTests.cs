using System;
using System.Linq;
using BlogDAL.Entities;
using BlogDALTests.RepositoryTests;
using BlogDALTests.TestsEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlogDALTests.RepositoryTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public void TestAdd()
        {
            var repository = new UserRepositoryBuilder().Build();

            repository.Add(new User { Id = 1, Name = "Frank", Email = "admin@admin", Password = "11111" });

            Assert.AreEqual("admin@admin", repository.GetById(1).Email);
        }

        [TestMethod]
        public void TestGetById_WhenUserExist_ShouldReturnUser()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.GetById(1);

            Assert.AreEqual("admin@admin", actual.Email);
        }

        [TestMethod]
        public void TestGetById_WhenUserDoesNotExist_ShouldReturnNull()
        {
            var repository = new UserRepositoryBuilder()
                .Build();

            var actual = repository.GetById(1);

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetAll_ShouldReturnEntities()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .WithUser(2, "Ivan", "user@user", "22222")
                .Build();

            var actual = repository.GetAll().ToList();

            Assert.IsTrue(actual.SequenceEqual(builder.UserSetBuilder.Data));
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        public void TestDelete_WhenUserExist()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();
            var user = builder.UserSetBuilder.GetData(1);

            repository.Delete(user);

            builder.UserSetBuilder.DbContext.Verify(c => c.Update(user));
            Assert.IsFalse(user.IsActive);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestDelete_WhenUserExist_ShouldExpectException()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder.Build();

            repository.Delete(new User { Id = 1, Name = "Frank", Email = "admin@admin", Password = "11111" });
        }

        [TestMethod]
        public void TestDeleteFromDb_WhenUserExist()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();
            var user = builder.UserSetBuilder.GetData(1);

            repository.DeleteFromDb(user);

            builder.UserSetBuilder.DbContext.Verify(c => c.Remove(user));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestDeleteFromDb_WhenUserExist_ShouldExpectException()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder.Build();

            repository.DeleteFromDb(new User { Id = 1, Name = "Frank", Email = "admin@admin", Password = "11111" });
        }

        [TestMethod]
        public void TestGet_ById_ShouldReturnEntities()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.Get(u => u.Id == 1);

            Assert.AreEqual(1, actual.Count());
        }

        [TestMethod]
        public void TestGet_ByEmail_ShouldReturnEntities()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.Get(u => u.Email == "admin@admin");

            Assert.AreEqual(1, actual.Count());
        }

        [TestMethod]
        public void TestGet_ByEmailAndPassword_ShouldReturnEntities()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.Get(u => u.Email == "admin@admin" && u.Password == "11111");

            Assert.AreEqual(1, actual.Count());
        }

        [TestMethod]
        public void TestGet_ById_WhenUserNonExist_ShouldReturnEmptyEnumerable()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.Get(u => u.Id == 100);

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestUpdate_WhenUserExist()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var user = builder.UserSetBuilder.GetData(1);

            repository.Update(user);

            builder.UserSetBuilder.DbContext.Verify(c => c.Update(user));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestUpdate_WhenUserNonExist_ShouldExpectException()
        {
            var repository = new UserRepositoryBuilder().Build();

            repository.Update(new User { Id = 1, Name = "Frank", Email = "admin@admin", Password = "11111" });
        }

        [TestMethod]
        public void TestGetNumberOfUserPosts_WhenUserExist_ShouldReturnZeroPosts()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.GetNumberOfUserPosts(1);
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void TestGetNumberOfUserPosts_WhenUserExist_ShouldReturnPostsCount()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .WithPost(1, "hi!", "hello!")
                .Build();

            var actual = repository.GetNumberOfUserPosts(1);
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void TestGetNumberOfUserComments_WhenUserExist_ShouldReturnZeroComments()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.GetNumberOfUserComments(1);
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void TestGetNumberOfUserComments_WhenUserExist_ShouldReturnCommentsCount()
        {
            var builder = new UserRepositoryBuilder();
            var repository = builder
                .WithUser(1, "Frank", "admin@admin", "11111")
                .WithComment(1, "fine")
                .Build();

            var actual = repository.GetNumberOfUserComments(1);
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void TestGetByEmail_WhenUserExist()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.GetByEmail("admin@admin");

            Assert.AreEqual("admin@admin", actual.Email);
        }

        [TestMethod]
        public void TestGetByEmail_WhenUserDoesNotExist_ShouldReturnNull()
        {
            var repository = new UserRepositoryBuilder()
                .Build();

            var actual = repository.GetByEmail("admin@admin");

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetByEmailAndPassword_WhenUserExist()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.GetByEmailAndPassword("admin@admin", "11111");

            Assert.AreEqual("admin@admin", actual.Email);
        }

        [TestMethod]
        public void TestGetByEmailAndPassword_WhenUserDoesNotExist_ShouldReturnNull()
        {
            var repository = new UserRepositoryBuilder()
                .Build();

            var actual = repository.GetByEmailAndPassword("admin@admin", "11111");

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetByName_WhenUserExist()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.GetByName("Frank");

            Assert.AreEqual("Frank", actual.Name);
        }

        [TestMethod]
        public void TestGetByName_WhenUserDoesNotExist_ShouldReturnNull()
        {
            var repository = new UserRepositoryBuilder()
                .WithUser(1, "Frank", "admin@admin", "11111")
                .Build();

            var actual = repository.GetByName("Ivan");

            Assert.AreEqual(null, actual);
        }
    }
}