using BlogDAL.Entities;
using BlogDAL.Interfaces;
using BlogDAL.Repositories;
using Moq;

namespace BlogDALTests.TestsEnvironment
{
    public class CommentRepositoryBuilder 
    {
        public DataSetBuilder<Comment, BlogContext> CommentSetBuilder { get; set; }

        public CommentRepositoryBuilder()
        {
            CommentSetBuilder = new DataSetBuilder<Comment, BlogContext>();
        }

        public CommentRepository Build()
        {
            CommentSetBuilder.SetOptions(c => c.Comments);
            return new CommentRepository(CommentSetBuilder.DbContext.Object);
        }

        public CommentRepositoryBuilder WithComment(int id, string text)
        {
            CommentSetBuilder.SetData(new Comment {Id = id, Text = text});
            return this;
        }
    }
}