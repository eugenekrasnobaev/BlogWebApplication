using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using BlogWebApplication.ViewModels;
using AutoMapper;
using BlogBLL.DTO;
using BlogBLL.Interfaces;
using BlogWebApplication.Filters;

namespace BlogWebApplication.Controllers
{
    [Authorize]
    [RequireHttps]
    [CustomExceptionFilter]
    public class HomeController : Controller
    {
        private readonly IBlogService blogService;
        private readonly IMapper mapper;

        public HomeController(IBlogService blogService, IMapper mapper)
        {
            this.blogService = blogService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PostsList()
        {
            var posts = blogService.PostsList();

            return View(posts);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        public IActionResult CreatePost(CreatePostVm model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelDto = mapper.Map<CreatePostVm, CreatePostDto>(model);

            blogService.CreatePost(modelDto, this.CurrentUserName());

            return RedirectToAction("PostsList", "Home");
        }

        [HttpGet]
        public IActionResult CreateComment()
        {
            return View();
        }

        public IActionResult CreateComment(CreateCommentVm model, int postId, string receiverName)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelDto = mapper.Map<CreateCommentVm, CreateCommentDto>(model);

            blogService.CreateComment(modelDto, this.CurrentUserName(), postId, receiverName);

            return RedirectToAction("PostsList", "Home");
        }

        public IActionResult DeleteComment(int commentId)
        {
            blogService.DeleteComment(commentId);

            return RedirectToAction("PostsList", "Home");
        }


        public IActionResult DeletePost(int postId)
        {
            blogService.DeletePost(postId);

            return RedirectToAction("PostsList", "Home");
        }

        [HttpGet]
        public IActionResult EditComment(int commentId)
        {
            return View(new EditCommentVm
            {
                Text = blogService.GetCommentById(commentId).Text
            });
        }

        [HttpGet]
        public IActionResult EditPost(int postId)
        {
            return View(new EditPostVm
            {
                Text = blogService.GetPostById(postId).Text
            });
        }

        public IActionResult EditComment(EditCommentVm model, int commentId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelDto = mapper.Map<EditCommentVm, EditCommentDto>(model);

            blogService.EditComment(modelDto, commentId);

            return RedirectToAction("PostsList", "Home");
        }

        public IActionResult EditPost(EditPostVm model, int postId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var modelDto = mapper.Map<EditPostVm, EditPostDto>(model);

            blogService.EditPost(modelDto, postId);

            return RedirectToAction("PostsList", "Home");
        }

        [HttpGet]
        public IActionResult UserInfo(int userId)
        {
            var userInfo = blogService.GetUserInfo(userId);
            return View(userInfo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVm { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
