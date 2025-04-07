using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Client
{
    public int IdClient { get; set; }

    public int? MemberId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Member? Member { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
