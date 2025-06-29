namespace eProfessional.BLL.DTOs
{
    public class ServiceDto
    {
        public int IdService { get; set; }
        public int? ProfessionalId { get; set; }
        public int? ServiceTypeId { get; set; }

        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
