using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class UserDto
    {
        public string? Username { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string? FirstName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "You need to enter the email")]
        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public required string Email { get; set; }
        [Phone(ErrorMessage = "Provide a correct phone number")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "You need to enter the password")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public required string Password { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
    }
}