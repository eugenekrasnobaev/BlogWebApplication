using System.ComponentModel.DataAnnotations;

namespace BlogWebApplication.ViewModels
{
    public class CreateCommentVm
    {
        [Required]
        public string Text { get; set; }

        public string ReceiverName { get; set; }
    }
}
