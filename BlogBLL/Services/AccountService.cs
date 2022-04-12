using AutoMapper;
using BlogBLL.DTO;
using BlogBLL.Interfaces;
using BlogDAL.Entities;
using BlogDAL.Interfaces;
using System;
using System.Collections.Generic;

namespace BlogBLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly IRepository<Post> postRepository;
        private readonly IRepository<Comment> commentRepository;
        private readonly IMapper mapper;

        public AccountService(IUserRepository userRepository,
                              IRepository<Post> postRepository,
                              IRepository<Comment> commentRepository,
                              IMapper mapper)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.mapper = mapper;
        }

        public bool DoesTheUserValid(string email, string password)
        {
            var user = userRepository.GetByEmailAndPassword(email, password);

            return user != null;
        }

        public bool DoesTheUserExist(string email)
        {
            var user = userRepository.GetByEmail(email);

            return user != null;
        }

        public UserDto CreateUser(RegisterUserDto model)
        {
            var user = mapper.Map<RegisterUserDto, User>(model);

            userRepository.Add(user);
            userRepository.Save();

            return mapper.Map<User, UserDto>(user);
        }

        public UserDto GetUserByEmail(string email)
        {
            var user = userRepository.GetByEmail(email);

            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            return mapper.Map<User, UserDto>(user);
        }

        public UserDto GetUserById(int userId)
        {
            var user = userRepository.GetById(userId);

            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            return mapper.Map<User, UserDto>(user);
        }

        public IEnumerable<UserDto> UsersList()
        {
            var users = userRepository.GetAll();
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        public void DeleteUser(int userId)
        {
            var user = userRepository.GetByIdWithInclude(userId, p => p.Posts, c => c.AuthorComments);

            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            DeleteRelatedComments(user, commentRepository.Delete);
            DeleteRelatedPosts(user, postRepository.Delete);

            userRepository.Delete(user);
            userRepository.Save();
        }

        private void DeleteRelatedPosts(User user, Action<Post> action)
        {
            user.Posts.ForEach(action);
            postRepository.Save();
        }

        private void DeleteRelatedComments(User user, Action<Comment> action)
        {
            user.AuthorComments.ForEach(action);
            commentRepository.Save();
        }

        public void EditUserPassword(EditUserDto model, int userId)
        {
            var user = userRepository.GetById(userId);

            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            user.Password = model.Password;
            userRepository.Update(user);
            userRepository.Save();
        }

        public Array GetRoles()
        {
            return Enum.GetValues(typeof(Role));
        }

        public void EditUserRole(EditUserDto model, int userId)
        {
            var user = userRepository.GetById(userId);

            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }
       
            user.Role = (Role) (model.Role == RoleDto.Admin ? RoleDto.User : RoleDto.Admin);
            userRepository.Update(user);
            userRepository.Save();
        }
    }
}
