using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CityIndexVM
    {
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, ErrorMessage = "City name too long.")]
        [Display(Name = "City name")]
        public string Name { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public List<CityVM> Cities { get; set; } = new List<CityVM>();
        public string SearchTerm { get; internal set; }
    }


    public class CityVM
    {

        public int Idcity { get; set; }
        [StringLength(100, ErrorMessage = "City name too long.")]
        [Display(Name = "City name")]
        public string Name { get; set; }
    }

    public class CityCreateVM
    {
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, ErrorMessage = "Maximum lenght for city name is 100 characters.")]
        [Display(Name = "City name")]
        public string Name { get; set; }
    }
}
