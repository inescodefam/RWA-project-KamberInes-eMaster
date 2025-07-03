using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class CityApiDto : CreateCityApiDto
    {
        public int? Idcity { get; set; }

    }

    public class CreateCityApiDto
    {
        [Required(ErrorMessage = "City name is required.")]

        public string Name { get; set; }
    }

}