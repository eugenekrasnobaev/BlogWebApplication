using System.ComponentModel.DataAnnotations;

namespace BlogWebApplication.ViewModels
{
    public class EditPostVm
    {
        [Required]
        public string Text { get; set; }
    }
}
