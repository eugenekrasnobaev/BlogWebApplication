using BlogDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogBLL.DTO
{
    public class RegisterUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public RoleDto Role { get; set; }

        public bool IsExternalAccount { get; set; }
    }
}
