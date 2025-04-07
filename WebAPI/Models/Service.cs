using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Service
{
    public int IdService { get; set; }

    public int? ProfessionalId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Professional? Professional { get; set; }
}
