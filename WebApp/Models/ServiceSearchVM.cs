namespace WebApp.Models
{
    public class ServiceSearchVM
    {
        public int? SelectedCityId { get; set; }
        public string? SelectedServiceTypeName { get; set; }

        //public List<CityVM>? Cities { get; set; }
        //public List<ServiceTypeVM>? ServiceTypes { get; set; }
        public List<ServiceTypeVM> ServiceTypes { get; set; } = new List<ServiceTypeVM>();
        public List<CityVM> Cities { get; set; } = new List<CityVM>();

        public List<ServiceResultVM>? Services { get; set; }
    }

    public class ServiceResultVM
    {
        public string? ProfessionalName { get; set; }
        public string? CityName { get; set; }
        public string? ServiceTypeName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
