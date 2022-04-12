using System.ComponentModel.DataAnnotations;

namespace BlogWebApplication.ViewModels
{
    public class EditCommentVm
    {
        [Required]
        public string Text { get; set; }
    }
}
