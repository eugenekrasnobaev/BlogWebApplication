using BlogDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogDAL.Entities
{
    public sealed class Post : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
        public int? AuthorId { get; set; }

        public List<Comment> Comments { get; set; }
        public Post()
        {
            Comments = new List<Comment>();
            Date = DateTime.Now;
            IsActive = true;
        }
    }
}
