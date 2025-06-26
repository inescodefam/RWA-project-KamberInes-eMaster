using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
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
        [Required]
        public int? SelectedProfessionalId { get; set; }
        public List<ProfessionalVM> Professionals { get; set; } = new List<ProfessionalVM>();
        [Required]
        public List<int>? SelectedCitiesIds { get; set; }
        [Required]
        public string? SelectedServiceTypeName { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        public List<CityVM> Cities { get; set; } = new List<CityVM>();
        public List<ServiceTypeVM> ServiceTypes { get; set; } = new List<ServiceTypeVM>();
    }

    public class ServiceEditVM
    {
        public int IdService { get; set; }
        public int? SelectedProfessionalId { get; set; }
        public List<int>? SelectedCityId { get; set; }
        public string? SelectedServiceTypeName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public List<ProfessionalVM> Professionals { get; set; } = new List<ProfessionalVM>();
        public List<CityVM> Cities { get; set; } = new List<CityVM>();
        public List<ServiceTypeVM>? ServiceTypes { get; set; }
    }
}
