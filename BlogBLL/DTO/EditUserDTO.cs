﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogBLL.DTO
{
    public class EditUserDto
    {
        public string Password { get; set; }

        public RoleDto Role { get; set; }
    }
}
