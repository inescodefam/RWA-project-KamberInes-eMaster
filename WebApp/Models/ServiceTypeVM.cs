using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ServiceTypeVM
    {
        public int? IdserviceType { get; set; }
        [Required(ErrorMessage = "Service type name is required.")]
        [StringLength(100, ErrorMessage = "Service type name cannot be longer than 100 characters.")]
        public string? ServiceTypeName { get; set; }
    }
}
