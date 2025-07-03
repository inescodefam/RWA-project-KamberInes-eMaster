namespace eProfessional.BLL.DTOs
{
    public class ProfessionalBaseDto
    {
        public int UserId { get; set; }
    }
    public class ProfessionalDto : ProfessionalBaseDto
    {
        public int IdProfessional { get; set; }
    }

    public class ProfessionalDataDto : ProfessionalDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
