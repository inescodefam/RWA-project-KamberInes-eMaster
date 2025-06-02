using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("Service")]
public partial class Service
{
    [Key]
    [Column("idService")]
    public int IdService { get; set; }

    [Column("professionalId")]
    public int? ProfessionalId { get; set; }

    [Column("serviceName")]
    [StringLength(100)]
    public string ServiceName { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Column("price", TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("ProfessionalId")]
    [InverseProperty("Services")]
    public virtual Professional? Professional { get; set; }
}
