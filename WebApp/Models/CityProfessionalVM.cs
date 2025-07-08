using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CityProfessionalVM
    {
        public int IdProfessionalCity { get; set; }

        public int ProfessionalId { get; set; }

        public int CityId { get; set; }

    }

    public class CityProfessionalsVm
    {
        public List<ProfessionalDataVM> Professionals { get; set; } = new List<ProfessionalDataVM>();
        public int? CityId { get; set; }

    }

    public class ProfessionalCityEditVM
    {
        public int IdProfessional { get; set; }
        public int UserId { get; set; }
        public List<int> CityIds { get; set; } = new List<int>();

    }

    public class CityProfessionalDataVM : CityProfessionalVM
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
    }

    public class ProfessionalIndexVM
    {
        [Display(Name = "Professional")]
        public List<ProfessionalVM> Professionals { get; set; }
        [Display(Name = "Users")]
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        [Display(Name = "Cities")]
        public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
    }
}
