using AutoMapper;
using BlogBLL.DTO;
using BlogDAL.Entities;
using BlogDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogBLL
{
    public class MapperBllProfile : Profile
    {
        public MapperBllProfile()
        {
            CreateMap<CreatePostDto, Post>();

            CreateMap<CreateCommentDto, Comment>();

            CreateMap<Post, PostDto>();

            CreateMap<Comment, CommentDto>();

            CreateMap<User, UserDto>();

            CreateMap<RegisterUserDto, User>();
        }
    }
}
