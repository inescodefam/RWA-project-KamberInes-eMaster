using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserVM
    {
        public int Iduser { get; set; }
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        [Display(Name = "Username")]
        public string? Username { get; set; }
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]

        [Display(Name = "Firstname")]
        public string? FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]

        [Display(Name = "Lastname")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone number")]
        public string? Phone { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be 8-100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
           ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }

    public class UserIndexVM
    {
        public List<UserVM> Users { get; set; } = new List<UserVM>();
        public string? Role { get; set; }
        public string? Username { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
    }
}
