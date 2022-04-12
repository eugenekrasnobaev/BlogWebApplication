using System.ComponentModel.DataAnnotations;

namespace BlogWebApplication.ViewModels
{
    public class EditUserPasswordVm
    {
        [Required]
        public string Password { get; set; }
    }
}
