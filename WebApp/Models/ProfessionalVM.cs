using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models
{
    public class ProfessionalIndexVM
    {
        public List<ProfessionalVM> Professionals { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public ProfessionalVM NewProfessional { get; set; }
    }

    public class ProfessionalVM
    {
        public int IdProfessional { get; set; }
        public int? UserId { get; set; }
        public List<int>? CityIds { get; set; }
        public List<CityVM> Cities { get; set; } = new List<CityVM>();
        public string? UserName { get; set; }
        public List<string>? CityNames { get; set; } = new List<string>();
    }
}
