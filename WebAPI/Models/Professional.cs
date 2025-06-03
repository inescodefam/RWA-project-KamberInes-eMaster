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

    public int? CityId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("experienceYears")]
    public int? ExperienceYears { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("Professionals")]
    public virtual City? City { get; set; }

    [InverseProperty("Professional")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    [ForeignKey("UserId")]
    [InverseProperty("Professionals")]
    public virtual User? User { get; set; }
}
