using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ProfessionalBaseVm
    {
        public int UserId { get; set; }
        public List<int> CityIds { get; set; }
    }

    public class ProfessionalVM : ProfessionalBaseVm
    {
        public int IdProfessional { get; set; }
        public string? UserName { get; set; }
        public List<string>? CityNames { get; set; } = new List<string>();
    }

    public class ProfessionalDataVM
    {
        public int IdProfessional { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username too long.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email too long.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number too long.")]
        [Display(Name = "Phone number")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name too long.")]
        [Display(Name = "First name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name too long.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }

    public class CreateProfessionalVM
    {
        [Required(ErrorMessage = "User does not exist in database.")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive integer.")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username too long.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email too long.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number too long.")]
        [Display(Name = "Phone number")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name too long.")]
        [Display(Name = "First name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name too long.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}