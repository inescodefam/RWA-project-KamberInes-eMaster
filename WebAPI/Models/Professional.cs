using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("Professional")]
public partial class Professional
{
    [Key]
    [Column("idProfessional")]
    public int IdProfessional { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("experienceYears")]
    public int? ExperienceYears { get; set; }

    [Column("rating")]
    public double? Rating { get; set; }

    [InverseProperty("Professional")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Professional")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Professional")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    [ForeignKey("UserId")]
    [InverseProperty("Professionals")]
    public virtual User? User { get; set; }
}
