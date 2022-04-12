using BlogDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogBLL.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public UserDto Author { get; set; }
        public int? AuthorId { get; set; }

        public ICollection<CommentDto> Comments { get; set; }

    }
}
