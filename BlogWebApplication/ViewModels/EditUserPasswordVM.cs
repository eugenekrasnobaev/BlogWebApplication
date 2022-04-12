using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApplication.ViewModels
{
    public class EditUserPasswordVm
    {
        [Required]
        public string Password { get; set; }
    }
}
