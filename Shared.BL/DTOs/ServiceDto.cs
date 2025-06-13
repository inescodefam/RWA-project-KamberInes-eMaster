namespace Shared.BL.DTOs
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public int? ProfessionalId { get; set; }
        public string? ServiceTypeId { get; set; }

        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
