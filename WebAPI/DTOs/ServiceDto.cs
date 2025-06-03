namespace WebAPI.DTOs
{
    public class ServiceDto
    {
        public int? ProfessionalId { get; set; }
        public string? ServiceTypeId { get; set; }

        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
