namespace WebAPI.DTOs
{
    public class ServiceApiDto
    {
        public int IdService { get; set; }
        public int? ProfessionalId { get; set; }
        public int? ServiceTypeId { get; set; }

        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }

    public class CreateServiceApiDto
    {
        public int? ProfessionalId { get; set; }
        public int? ServiceTypeId { get; set; }

        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
