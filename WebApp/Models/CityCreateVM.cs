using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CityCreateVM
    {
        [Required(ErrorMessage = "City name is required.")]
        [StringLength(100, ErrorMessage = "Naziv grada može imati najviše 100 znakova.")]
        [Display(Name = "Naziv grada")]
        public string Name { get; set; }
    }
}