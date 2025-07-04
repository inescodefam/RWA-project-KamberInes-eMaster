using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProfessional.DAL.Models;
[Table("ServiceType")]
public partial class ServiceType
{
    [Key]
    [Column("IDServiceType")]
    public int IdserviceType { get; set; }

    [Column("ServiceType")]
    [StringLength(100)]
    public string ServiceTypeName { get; set; }

    [InverseProperty("ServiceType")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
