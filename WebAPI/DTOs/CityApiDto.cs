using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class CityApiDto
    {
        public int? Idcity { get; set; }
        [Required(ErrorMessage = "City name is required.")]
        public string Name { get; set; }
    }

    public class CreateCityApiDto
    {
        [Required(ErrorMessage = "City name is required.")]

        public string Name { get; set; }
    }
}