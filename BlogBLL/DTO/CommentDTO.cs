using System;

namespace BlogBLL.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public UserDto Author { get; set; }
        public int? AuthorId { get; set; }

        public UserDto Receiver { get; set; }
        public int? ReceiverId { get; set; }

        public PostDto Post { get; set; }
        public int? PostId { get; set; }
    }
}
