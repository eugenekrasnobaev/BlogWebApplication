using BlogDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDAL.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
        public int? AuthorId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }
        public int? ReceiverId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public int? PostId { get; set; }

        public Comment()
        {
            Date = DateTime.Now;
            IsActive = true;
        }
    }
}
