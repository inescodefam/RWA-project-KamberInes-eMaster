using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;

namespace WebAPI.DTOs
{
    public class ServiceDto
    {
        public int? ProfessionalId { get; set; }
        public string ServiceName { get; set; } = null!;

        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public virtual Professional? Professional { get; set; }
    }
}
