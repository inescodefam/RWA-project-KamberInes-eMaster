using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProfessional.DAL.Models;
[Table("Role")]
public partial class Role
{
    [Key]
    [Column("IDRole")]
    public int Idrole { get; set; }

    [Column("userId")]
    public int? UserId { get; set; }

    [Column("roleName")]
    [StringLength(50)]
    public string? RoleName { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Roles")]
    public virtual User? User { get; set; }
}
