namespace WebAPI.DTOs
{
    public class ServiceDto
    {
        public int? ProfessionalId { get; set; }
        public string ServiceName { get; set; } = null!;

        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
