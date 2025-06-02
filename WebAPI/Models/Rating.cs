using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[PrimaryKey("ProfessionalId", "UserId")]
[Table("Rating")]
public partial class Rating
{
    [Key]
    [Column("professionalId")]
    public int ProfessionalId { get; set; }

    [Key]
    [Column("userId")]
    public int UserId { get; set; }

    [Column("rate")]
    public int? Rate { get; set; }

    [ForeignKey("ProfessionalId")]
    [InverseProperty("Ratings")]
    public virtual Professional Professional { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Ratings")]
    public virtual User User { get; set; } = null!;
}
