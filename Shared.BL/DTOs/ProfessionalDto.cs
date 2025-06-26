namespace Shared.BL.DTOs
{
    public class ProfessionalDto
    {
        public int IdProfessional { get; set; }
        public int? UserId { get; set; }
        public int? ExperienceYears { get; set; }
        public List<CityDto> Cities { get; set; } = new List<CityDto>();
    }
}
