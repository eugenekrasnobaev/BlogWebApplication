using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApplication.ViewModels
{
    public class EditPostVm
    {
        [Required]
        public string Text { get; set; }
    }
}
