using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class CityProfessionalApiDto
    {
        public int IdProfessionalCity { get; set; }

        public int ProfessionalId { get; set; }

        public int CityId { get; set; }

    }


    public class CreateCityProfessionalApiDto
    {
        [Required(ErrorMessage = "Professional must be defined!")]
        public int ProfessionalId { get; set; }
        [Required(ErrorMessage = "City must be defined!")]
        public int CityId { get; set; }

    }

    public class CityProfessionalDataApiDto : CityProfessionalApiDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
    }
}
