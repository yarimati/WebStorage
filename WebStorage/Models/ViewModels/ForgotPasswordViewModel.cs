using System.ComponentModel.DataAnnotations;

namespace WebStorage.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
