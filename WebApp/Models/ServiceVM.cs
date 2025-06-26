using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ServiceVM
    {
        public int IdService { get; set; }
        [Required(ErrorMessage = "Professional is required.")]
        public int? ProfessionalId { get; set; }
        [Required(ErrorMessage = "Service ype is required.")]
        public int? ServiceTypeId { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value.")]
        public decimal? Price { get; set; }
    }
}
