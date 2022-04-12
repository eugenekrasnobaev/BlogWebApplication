using AutoMapper;
using BlogBLL.DTO;
using BlogWebApplication.ViewModels;

namespace BlogWebApplication
{
    public class MapperPlProfile : Profile
    {
        public MapperPlProfile()
        {
            CreateMap<CreatePostVm, CreatePostDto>();

            CreateMap<CreateCommentVm, CreateCommentDto>();

            CreateMap<EditPostVm, EditPostDto>();

            CreateMap<EditCommentVm, EditCommentDto>();

            CreateMap<LoginUserVm, LoginUserDto>();

            CreateMap<RegisterUserVM, RegisterUserDto>();

            CreateMap<EditUserPasswordVm, EditUserDto>();

            CreateMap<EditUserRoleVM, EditUserDto>();
        }
    }
}
