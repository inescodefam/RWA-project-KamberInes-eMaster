using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AuthVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 8-100 characters.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string? ConfirmPassword { get; set; }

    }
}
