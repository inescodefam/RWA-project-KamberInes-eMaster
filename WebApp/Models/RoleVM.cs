using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RoleVM
    {

        public int Idrole { get; set; }
        [Required(ErrorMessage = "User mus be valid user.")]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Role must have valid name.")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
