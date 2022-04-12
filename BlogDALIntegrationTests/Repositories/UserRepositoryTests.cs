using System.Linq;
using BlogDAL.Entities;
using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlogDALIntegrationTests.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private IUserRepository repository;

        [TestInitialize]
        public void Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogContext>();

            var options = optionsBuilder
                .UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = blogdb; Trusted_Connection = True;")
                .Options;

            repository = new UserRepository( new BlogContext(options));
        }

        public User TestAdd()
        {
            repository.Add(new User { Email = "test@test", Password = "11111" });
            repository.Save();

            var actual = repository.GetByEmail("test@test");

            Assert.AreEqual("test@test", actual.Email, "Create fail");
            return actual;
        }

        public void TestGetById(User user)
        {
            var actual = repository.GetById(user.Id);

            Assert.AreEqual("test@test", actual.Email, "Get fail");
        }

        public void TestGetAll()
        {
            var users = repository.GetAll();

            Assert.IsTrue(users.Any(), "GetAll fail");
        }

        public void TestDelete(User user)
        {
            repository.Delete(user);
            repository.Save();

            var actual = repository.GetById(user.Id);
            Assert.AreEqual(null, actual, "Delete fail");
        }

        public void TestUpdate(User user, string newPassword)
        {
            user.Password = newPassword;
            repository.Update(user);
            repository.Save();

            var actual = repository.GetById(user.Id);

            Assert.AreEqual(newPassword, actual.Password, "Update fail");
        }

        [TestMethod]
        public void TestScript()
        {
            var user = TestAdd();
            TestGetById(user);
            TestGetAll();
            TestUpdate(user, "54321");
            TestDelete(user);
        }
    }
}