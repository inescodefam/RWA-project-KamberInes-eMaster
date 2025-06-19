namespace WebApp.Models
{
    public class ServiceVM
    {
        public int IdService { get; set; }
        public int? ProfessionalId { get; set; }
        public int? ServiceTypeId { get; set; }

        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
