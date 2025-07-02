using Microsoft.Build.Framework;

namespace WebAPI.DTOs
{
    public class ServiceApiDto
    {
        public int IdService { get; set; }
        [Required]
        public int ProfessionalId { get; set; }
        [Required]
        public int ServiceTypeId { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
    }

    public class CreateServiceApiDto
    {
        [Required]
        public int ProfessionalId { get; set; }
        [Required]
        public int ServiceTypeId { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
    }
}
