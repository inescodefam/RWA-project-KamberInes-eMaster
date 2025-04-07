using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Professional
{
    public int IdProfessional { get; set; }

    public int? MemberId { get; set; }

    public string? Category { get; set; }

    public int? ExperienceYears { get; set; }

    public double? Rating { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Member? Member { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
