using BlogDAL.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogDAL.Entities
{
    public sealed class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsExternalAccount { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }

        public List<Post> Posts { get; set; }

        [InverseProperty("Author")]
        public List<Comment> AuthorComments { get; set; }

        [InverseProperty("Receiver")]
        public List<Comment> CommentsToAuthor { get; set; }

        public User()
        {
            Posts = new List<Post>();
            AuthorComments = new List<Comment>();
            CommentsToAuthor = new List<Comment>();
            IsActive = true;
        }
    }
}
