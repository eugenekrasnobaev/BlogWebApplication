using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogBLL.DTO
{
    public class UserInfoDto
    {
        public UserDto User { get; set; }

        public int NumberOfUserPosts { get; set; }

        public int NumberOfUserComments { get; set; }
    }
}
