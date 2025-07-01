namespace eProfessional.BLL.DTOs
{
    public class CityProfessionalDto
    {
        public int IdProfessionalCity { get; set; }

        public int ProfessionalId { get; set; }

        public int CityId { get; set; }

    }

    public class CityProfessionalDataDto : CityProfessionalDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
    }
}
