using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("Booking")]
public partial class Booking
{
    [Key]
    [Column("idBooking")]
    public int IdBooking { get; set; }

    [Column("userId")]
    public int? UserId { get; set; }

    [Column("professionalId")]
    public int? ProfessionalId { get; set; }

    [Column("serviceId")]
    public int? ServiceId { get; set; }

    [Column("bookingDate")]
    public DateTime BookingDate { get; set; }

    [Column("bookingStatus")]
    [StringLength(20)]
    public string? BookingStatus { get; set; }

    [Column("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ProfessionalId")]
    [InverseProperty("Bookings")]
    public virtual Professional? Professional { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("Bookings")]
    public virtual Service? Service { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual User? User { get; set; }
}
