using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("Review")]
[Index("ProfessionalId", Name = "idxReviewsProfessional")]
public partial class Review
{
    [Key]
    [Column("idReview")]
    public int IdReview { get; set; }

    [Column("userId")]
    public int? UserId { get; set; }

    [Column("professionalId")]
    public int? ProfessionalId { get; set; }

    [Column("rating")]
    public int? Rating { get; set; }

    [Column("comment")]
    [StringLength(255)]
    public string? Comment { get; set; }

    [Column("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ProfessionalId")]
    [InverseProperty("Reviews")]
    public virtual Professional? Professional { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Reviews")]
    public virtual User? User { get; set; }
}
