using System;
using System.Collections.Generic;
using System.Linq;
using BlogBLL.DTO;
using BlogDAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlogBLLTests.Services
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public void TestDoesTheUserValid_WhenUserExists()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            builder.UserRepository.Setup(r => r.GetByEmailAndPassword("admin@admin", "1111"))
                .Returns(new User());

            var actual = service.DoesTheUserValid("admin@admin", "1111");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestDoesTheUserValid_WhenUserDoesNotExist()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            builder.UserRepository.Setup(r => r.GetByEmailAndPassword("user@user", "1111"))
                .Returns((User)null);

            var actual = service.DoesTheUserValid("admin@admin", "1111");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void TestDoesTheUserExist()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            builder.UserRepository.Setup(r => r.GetByEmail("admin@admin")).Returns(new User());
            
            var actual = service.DoesTheUserExist("admin@admin");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestDoesTheUserExist_WhenUserDoesNotExist()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            builder.UserRepository.Setup(r => r.GetByEmailAndPassword("user@user", "1111"))
                .Returns((User)null);

            var actual = service.DoesTheUserExist("admin@admin");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void TestCreateUser()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();

            var registeredUserDto = new RegisterUserDto
                { Id = 1, Email = "user@user", Password = "11111", Role = RoleDto.User, IsExternalAccount = false };
            
            var actual = service.CreateUser(registeredUserDto);

            Assert.AreEqual("user@user", actual.Email);
            builder.UserRepository.Verify(r => r.Add(It.Is<User>(u => u.Id == 1)));
        }

        [TestMethod]
        public void TestDeleteUser_WhenUserExists()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            var user = new User { Id = 1, Email = "user@user", Password = "11111" };
            var post = new Post { Id = 1, Title = "Hi!", Text = "Hello!", AuthorId = 1};
            var comment = new Comment { Id = 1, Text = "Hello!", PostId = 1, AuthorId = 1, ReceiverId = 1};
            user.Posts.Add(post);
            user.AuthorComments.Add(comment);

            builder.UserRepository.Setup(r => r.GetByIdWithInclude(1, p => p.Posts, c => c.AuthorComments))
                .Returns(user);

            service.DeleteUser(1);

            builder.UserRepository.Verify(r => r.Delete(user));
            builder.PostRepository.Verify(r => r.Delete(post));
            builder.CommentRepository.Verify(r => r.Delete(comment));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteUser_WhenUserDoesNotExist_ShouldExpectException()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();

            service.DeleteUser(1);
        }
        
        [TestMethod]
        public void TestEditUser_WhenUserExists()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            var user = new User { Id = 1, Email = "user@user", Password = "11111" };
            builder.UserRepository.Setup(r => r.GetById(1)).Returns(user);

            service.EditUserPassword(new EditUserDto { Password = "12345" }, 1);

            Assert.AreEqual("12345", user.Password);
            builder.UserRepository.Verify(r => r.Update(user));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEditUser_WhenNoUsers_ShouldExpectException()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();

            service.EditUserPassword(new EditUserDto(), 1);
        }
        
        [TestMethod]
        public void TestUsersList() 
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            var users = new List<User>
            {
                new User { Id = 1, Email = "user@user", Password = "11111" },
                new User { Id = 2, Email = "admin@admin", Password = "22222" }
            };

            builder.UserRepository.Setup(r => r.GetAll()).Returns(users);

            var actual = service.UsersList().ToList();

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual("user@user", actual.Single(u => u.Id == 1).Email);
        }
       
        [TestMethod]
        public void TestUsersList_WhenNoUser_ShouldReturnEmptyEnumerable()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();

            var actual = service.UsersList();

            Assert.AreEqual(0, actual.Count());
        }
        
        [TestMethod]
        public void TestGetRoles()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();

            var actual = service.GetRoles();

            Assert.AreEqual(2, actual.Length);
        }
        
        [TestMethod]
        public void TestEditUserRole_WhenUserExists()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();
            var user = new User { Id = 1, Email = "user@user", Password = "11111" };
            builder.UserRepository.Setup(r => r.GetById(1)).Returns(user);

            service.EditUserRole( new EditUserDto{ Role = RoleDto.Admin }, 1);

            builder.UserRepository.Verify(r => r.Update(user));
            Assert.AreEqual(Role.Admin, user.Role);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEditUserRole_WhenUserNotExists_ShouldExpectException()
        {
            var builder = new AccountServiceBuilder();
            var service = builder.Create();

            service.EditUserRole(new EditUserDto { Role = RoleDto.Admin }, 1);
        }
    }
}
