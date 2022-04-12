using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BlogBLL;
using BlogBLL.Interfaces;
using BlogBLL.Services;
using BlogDAL.Entities;
using BlogDAL.Interfaces;
using Moq;

namespace BlogBLLTests
{
    public class AccountServiceBuilder 
    {
        private bool useMapperMock;

        public AccountService Create()
        {
            return new AccountService(UserRepository.Object, PostRepository.Object,
                CommentRepository.Object, SelectMapper());
        }

        public AccountServiceBuilder UseMapperMock()
        {
            useMapperMock = true;
            return this;
        }

        public Mock<IUserRepository> UserRepository { get; set; } = new Mock<IUserRepository>();

        public Mock<IRepository<Post>> PostRepository { get; set; } = new Mock<IRepository<Post>>();

        public Mock<IRepository<Comment>> CommentRepository { get; set; } = new Mock<IRepository<Comment>>();

        public Mock<IMapper> Mapper { get; set; } = new Mock<IMapper>();

        private IMapper SelectMapper()
        {
            return useMapperMock ? Mapper.Object : CreateRealMapper();
        }

        private static IMapper CreateRealMapper()
        {
            var realMapper = new MapperConfiguration(cfg => { cfg.AddProfile<MapperBllProfile>(); }).CreateMapper();
            return realMapper;
        }
    }
}
