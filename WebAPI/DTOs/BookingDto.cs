namespace WebAPI.DTOs
{
    public class BookingDto
    {
        public int? UserId { get; set; }
        public int? ProfessionalId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime BookingDate { get; set; }
        public string? BookingStatus { get; set; }
    }
}
