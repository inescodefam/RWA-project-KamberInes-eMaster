using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.BL.Models;

[Table("City")]
public partial class City
{
    [Key]
    [Column("IDCity")]
    public int Idcity { get; set; }

    [Column("City")]
    [StringLength(100)]
    public string? Name { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();
}
