using AutoMapper;
using BlogBLL.DTO;
using BlogBLL.Interfaces;
using BlogDAL.Entities;
using BlogDAL.Interfaces;
using System;
using System.Collections.Generic;

namespace BlogBLL.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUserRepository userRepository;
        private readonly IRepository<Post> postRepository;
        private readonly IRepository<Comment> commentRepository;
        private readonly IMapper mapper;

        public BlogService(IUserRepository userRepository,
                           IRepository<Post> postRepository,
                           IRepository<Comment> commentRepository,
                           IMapper mapper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }

        public void CreatePost(CreatePostDto model, string userName)
        {
            var user = userRepository.GetByName(userName);

            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            var post = mapper.Map<CreatePostDto, Post>(model,
                opt => { opt.AfterMap((src, dest) => dest.AuthorId = user.Id); });

            postRepository.Add(post);
            postRepository.Save();
        }

        public void CreateComment(CreateCommentDto model, string authorName, int postId, string receiverName)
        {
            var authorUser = userRepository.GetByName(authorName);

            if (authorUser == null)
            {
                throw new ArgumentException("Author not exist!");
            }

            var receiverUser = userRepository.GetByName(receiverName);

            var comment = mapper.Map<CreateCommentDto, Comment>(model,
                opt => { opt.AfterMap((src, dest) => { dest.AuthorId = authorUser.Id; dest.PostId = postId; dest.ReceiverId = receiverUser?.Id; }); });

            commentRepository.Add(comment);
            commentRepository.Save();
        }

        public void DeleteComment(int commentId)
        {
            var comment = commentRepository.GetById(commentId);

            if (comment == null)
            {
                throw new ArgumentException("Comment not exist!");
            }

            commentRepository.Delete(comment);
            commentRepository.Save();
        }

        public IEnumerable<PostDto> PostsList()
        {
            return mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(postRepository.GetAll());
        }

        public void DeletePost(int postId)
        {
            var post = postRepository.GetByIdWithInclude(postId, p => p.Comments);

            if (post == null)
            {
                throw new ArgumentException("Post not exist!");
            }

            DeactivateRelatedComments(post);

            postRepository.Delete(post);
            postRepository.Save();
        }

        public void EditComment(EditCommentDto model, int commentId)
        {
            var comment = commentRepository.GetById(commentId);

            if (comment == null)
            {
                throw new ArgumentException("Comment not exist!");
            }

            comment.Text = model.Text;
            commentRepository.Update(comment);
            commentRepository.Save();
        }

        public void EditPost(EditPostDto model, int postId)
        {
            var post = postRepository.GetById(postId);

            if (post == null)
            {
                throw new ArgumentException("Post not exist!");
            }

            post.Text = model.Text;
            postRepository.Update(post);
            postRepository.Save();
        }

        public PostDto GetPostById(int postId)
        {
            var post = postRepository.GetById(postId);

            if (post == null)
            {
                throw new ArgumentException("Post not exist!");
            }

            return mapper.Map<Post, PostDto>(post);
        }

        public CommentDto GetCommentById(int commentId)
        {
            var comment = commentRepository.GetById(commentId);

            if (comment == null)
            {
                throw new ArgumentException("Comment not exist!");
            }

            return mapper.Map<Comment, CommentDto>(comment);
        }

        private void DeactivateRelatedComments(Post post)
        {
            post.Comments.ForEach(c => commentRepository.Delete(c));
            commentRepository.Save();
        }

        public UserInfoDto GetUserInfo(int userId)
        {
            var user = userRepository.GetById(userId);

            if (user == null)
            {
                throw new ArgumentException("User not exist!");
            }

            return new UserInfoDto
            {
                User = mapper.Map<User, UserDto>(user),
                NumberOfUserPosts = userRepository.GetNumberOfUserPosts(userId),
                NumberOfUserComments = userRepository.GetNumberOfUserComments(userId)
            };
        }
    }
}
