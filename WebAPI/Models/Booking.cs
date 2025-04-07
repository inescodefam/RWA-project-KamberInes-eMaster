using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Booking
{
    public int IdBooking { get; set; }

    public int? UserId { get; set; }

    public int? ProfessionalId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime BookingDate { get; set; }

    public string? BookingStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Professional? Professional { get; set; }

    public virtual Service? Service { get; set; }

    public virtual Client? User { get; set; }
}
