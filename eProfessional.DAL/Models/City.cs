﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProfessional.DAL.Models;

[Table("City")]
public partial class City
{
    [Key]
    [Column("IDCity")]
    public int Idcity { get; set; }

    [Column("City")]
    [StringLength(100)]
    public string? Name { get; set; }

    public virtual ICollection<CityProfessional> CityProfessionals { get; set; } = new List<CityProfessional>();
}
