namespace eProfessional.BLL.DTOs
{

    public class LogDto
    {
        public int IdLog { get; set; }

        public DateTime? LogTimeStamp { get; set; }

        public string? LogMessage { get; set; }

        public string? LogLevel { get; set; }
    }

}
