using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class ServiceTypeApiDto
    {
        public int IdserviceType { get; set; }
        public string? ServiceTypeName { get; set; }
    }

    public class CreateServiceTypeApiDto
    {
        [Required(ErrorMessage = "Service type name must be provided.")]
        public string ServiceTypeName { get; set; }
    }
}