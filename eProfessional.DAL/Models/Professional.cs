using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProfessional.DAL.Models;
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

    public virtual ICollection<CityProfessional> CityProfessionals { get; set; } = new List<CityProfessional>();


    [InverseProperty("Professional")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    [ForeignKey("UserId")]
    [InverseProperty("Professionals")]
    public virtual User? User { get; set; }
}
