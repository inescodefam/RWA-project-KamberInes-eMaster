using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CityCreateVM
    {
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, ErrorMessage = "Maximum lenght for city name is 100 characters.")]
        [Display(Name = "City name")]
        public string Name { get; set; }
    }
}