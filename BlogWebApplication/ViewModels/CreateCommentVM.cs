using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApplication.ViewModels
{
    public class CreateCommentVm
    {
        [Required]
        public string Text { get; set; }

        public string ReceiverName { get; set; }
    }
}
