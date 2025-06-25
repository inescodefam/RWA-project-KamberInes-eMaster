using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AuthVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email adresa")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati najmanje 6 znakova.")]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        public string? ConfirmPassword { get; set; }

    }
}
