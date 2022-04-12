using BlogDAL.Entities;
using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using BlogDALTests.TestsEnvironment;
using Moq;

namespace BlogDALTests.TestsEnvironment
{
    public class PostRepositoryBuilder
    {
        public DataSetBuilder<Comment, BlogContext> CommentSetBuilder { get; set; }
        public DataSetBuilder<Post, BlogContext> PostSetBuilder { get; set; }

        public Mock<BlogContext> DbContext { get; set; } = new Mock<BlogContext>();

        public PostRepositoryBuilder()
        {
            PostSetBuilder = new DataSetBuilder<Post, BlogContext>(DbContext);
            CommentSetBuilder = new DataSetBuilder<Comment, BlogContext>(DbContext);
        }
        public PostRepository Build()
        {
            CommentSetBuilder.SetOptions(c => c.Comments);
            PostSetBuilder.SetOptions(c => c.Posts);
            return new PostRepository(DbContext.Object);
        }

        public PostRepositoryBuilder WithPost(int id, string title, string text)
        {
            var post = new Post {Id = id, Title = title, Text = text};
            PostSetBuilder.SetData(post);
            return this;
        }
    }
}