using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Review
{
    public int IdReview { get; set; }

    public int? UserId { get; set; }

    public int? ProfessionalId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Professional? Professional { get; set; }

    public virtual Client? User { get; set; }
}
