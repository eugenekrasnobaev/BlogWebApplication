﻿using System.ComponentModel.DataAnnotations;

namespace BlogWebApplication.ViewModels
{
    public class LoginUserVm
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
