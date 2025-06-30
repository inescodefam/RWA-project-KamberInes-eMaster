using System.ComponentModel.DataAnnotations;

namespace eProfessional.BLL.DTOs
{
    public class ServiceDto
    {
        public int IdService { get; set; }
        [Required(ErrorMessage = "Professional is required!")]
        public int ProfessionalId { get; set; }
        [Required(ErrorMessage = "Service type is required!")]
        public int ServiceTypeId { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required!")]
        public decimal Price { get; set; }
    }
}
