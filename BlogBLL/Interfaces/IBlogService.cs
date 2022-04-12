using BlogBLL.DTO;
using System.Collections.Generic;

namespace BlogBLL.Interfaces
{
    public interface IBlogService
    {
        void CreatePost(CreatePostDto model, string userName);
        void CreateComment(CreateCommentDto model, string authorName, int postId, string receiverName);
        IEnumerable<PostDto> PostsList();
        void DeleteComment(int commentId);
        void DeletePost(int postId);
        void EditComment(EditCommentDto model, int commentId);
        void EditPost(EditPostDto model, int postId);
        PostDto GetPostById(int postId);
        CommentDto GetCommentById(int commentId);
        UserInfoDto GetUserInfo(int userId);
    }
}
