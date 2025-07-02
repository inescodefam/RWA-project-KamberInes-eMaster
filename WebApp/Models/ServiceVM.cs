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

    public class ServiceSearchVM
    {
        public int? SelectedCityId { get; set; }
        public string? SelectedServiceTypeName { get; set; }
        public List<ServiceTypeVM> ServiceTypes { get; set; } = new List<ServiceTypeVM>();
        public List<CityVM> Cities { get; set; } = new List<CityVM>();
        public List<ServiceResultVM>? Services { get; set; }
    }

    public class ServiceResultVM
    {
        public int? IdService { get; set; }
        public string? ProfessionalName { get; set; }
        public List<string>? CityNames { get; set; }
        public string? ServiceTypeName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }

    public class ServiceCreateVM
    {
        [Required(ErrorMessage = "Professional selection is required.")]
        [Display(Name = "Professional")]
        public int? SelectedProfessionalId { get; set; }
        public List<ProfessionalVM> Professionals { get; set; } = new List<ProfessionalVM>();

        [Required(ErrorMessage = "At least one city must be selected.")]
        [MinLength(1, ErrorMessage = "At least one city must be selected.")]
        [Display(Name = "Cities")]
        public List<int>? SelectedCitiesIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Service type selection is required.")]
        [Display(Name = "Service type")]
        public int SelectedServiceTypeId { get; set; }

        [Required(ErrorMessage = "Service type selection is required.")]
        [StringLength(100, ErrorMessage = "Service type name cannot be longer than 100 characters.")]
        [Display(Name = "Service type")]
        public string? SelectedServiceTypeName { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        public List<CityVM> Cities { get; set; } = new List<CityVM>();

        public List<ServiceTypeVM> ServiceTypes { get; set; } = new List<ServiceTypeVM>();
    }

    public class ServiceEditVM
    {
        public int IdService { get; set; }
        [Display(Name = "Professional")]
        public int SelectedProfessionalId { get; set; }
        [MinLength(1, ErrorMessage = "At least one city must be selected.")]
        [Display(Name = "Cities")]
        public List<int>? SelectedCityId { get; set; }
        [StringLength(100, ErrorMessage = "Service type name cannot be longer than 100 characters.")]
        [Display(Name = "Service type")]
        public int SelectedServiceTypeId { get; set; }
        public string? SelectedServiceTypeName { get; set; }
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        public List<ProfessionalVM> Professionals { get; set; } = new List<ProfessionalVM>();
        public List<CityVM> Cities { get; set; } = new List<CityVM>();
        public List<ServiceTypeVM>? ServiceTypes { get; set; }
    }
}
