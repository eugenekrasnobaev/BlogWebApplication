using System.ComponentModel.DataAnnotations;

namespace BlogWebApplication.ViewModels
{
    public class CreatePostVm
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
