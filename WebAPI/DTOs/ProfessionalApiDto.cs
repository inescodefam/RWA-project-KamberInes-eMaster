namespace WebAPI.DTOs
{
    public class ProfessionalBaseApiDto
    {
        public int UserId { get; set; }
    }
    public class ProfessionalApiDto : ProfessionalBaseApiDto
    {
        public int IdProfessional { get; set; }
    }

    public class ProfessionalApiDataDto : ProfessionalApiDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CreateProfessionalApiDataDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
