using System;
using System.Linq;
using BlogDAL.Entities;
using BlogDALTests.TestsEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlogDALTests.RepositoryTests
{
    [TestClass]
    public class CommentRepositoryTests
    {
        [TestMethod]
        public void TestAdd()
        {
            var repository = new CommentRepositoryBuilder().Build();

            repository.Add(new Comment {Id = 1, Text = "Need proofs!"});

            Assert.AreEqual("Need proofs!", repository.GetById(1).Text);
        }

        [TestMethod]
        public void TestGetById_WhenCommentExist_ShouldReturnComment()
        {
            var repository = new CommentRepositoryBuilder()
                .WithComment(1, "Need proofs!")
                .Build();

            var actual = repository.GetById(1);

            Assert.AreEqual("Need proofs!", actual.Text);
        }

        [TestMethod]
        public void TestGetById_WhenCommentDoesNotExist_ShouldReturnNull()
        {
            var repository = new CommentRepositoryBuilder()
                .Build();

            var actual = repository.GetById(1);

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetById_WhenCommentNotExist_ShouldReturnNull()
        {
            var repository = new CommentRepositoryBuilder().Build();

            var actual = repository.GetById(1);

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void TestGetAll_ShouldReturnEntities()
        {
            var builder = new CommentRepositoryBuilder();
            var repository = builder
                .WithComment(1, "Need proofs!")
                .WithComment(2, "UG!")
                .Build();

            var actual = repository.GetAll().ToList();

            Assert.IsTrue(actual.SequenceEqual(builder.CommentSetBuilder.Data));
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        public void TestGet_ById_ShouldReturnEntities()
        {
            var repository = new CommentRepositoryBuilder()
                .WithComment(1, "Need proofs!")
                .Build();

            var actual = repository.Get(c => c.Id == 1);

            Assert.AreEqual(1, actual.Count());
        }

        [TestMethod]
        public void TestGet_ByText_ShouldReturnEntities()
        {
            var repository = new CommentRepositoryBuilder()
                .WithComment(1, "Need proofs!")
                .Build();

            var actual = repository.Get(u => u.Text == "Need proofs!");

            Assert.AreEqual(1, actual.Count());
        }


        [TestMethod]
        public void TestGet_ById_WhenCommentNonExist_ShouldReturnEmptyEnumerable()
        {
            var repository = new CommentRepositoryBuilder()
                .WithComment(1, "Need proofs!")
                .Build();

            var actual = repository.Get(u => u.Id == 100);

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestUpdate_WhenCommentExist()
        {
            var builder = new CommentRepositoryBuilder();
            var repository = builder
                .WithComment(1, "Need proofs!")
                .Build();

            var comment = builder.CommentSetBuilder.GetData(1);

            repository.Update(comment);

            builder.CommentSetBuilder.DbContext.Verify(c => c.Update(comment));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestUpdate_WhenCommentNonExist_ShouldExpectException()
        {
            var repository = new CommentRepositoryBuilder().Build();

            repository.Update(new Comment {Id = 1, Text = "Need proofs!"});
        }

        [TestMethod]
        public void TestDelete_WhenCommentExist()
        {
            var builder = new CommentRepositoryBuilder();
            var repository = builder
                .WithComment(1, "Hello!")
                .Build();
            var comment = builder.CommentSetBuilder.GetData(1);

            repository.Delete(comment);

            builder.CommentSetBuilder.DbContext.Verify(c => c.Update(comment));
            Assert.IsFalse(comment.IsActive);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestDelete_WhenCommentNonExist_ShouldExpectException()
        {
            var repository = new CommentRepositoryBuilder().Build();

            repository.Delete(new Comment { Id = 1, Text = "Hello" });
        }

        [TestMethod]
        public void TestDeleteFromDb_WhenUserExist()
        {
            var builder = new CommentRepositoryBuilder();
            var repository = builder
                .WithComment(1, "Hello!")
                .Build();
            var comment = builder.CommentSetBuilder.GetData(1);

            repository.DeleteFromDb(comment);

            builder.CommentSetBuilder.DbContext.Verify(c => c.Remove(comment));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestDeleteFromDb_WhenUserExist_ShouldExpectException()
        {
            var builder = new CommentRepositoryBuilder();
            var repository = builder.Build();

            repository.DeleteFromDb(new Comment { Id = 1, Text = "Hello" });
        }
    }
}