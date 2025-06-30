namespace eProfessional.BLL.DTOs
{
    public class UserDto
    {
        public int Iduser { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }
}