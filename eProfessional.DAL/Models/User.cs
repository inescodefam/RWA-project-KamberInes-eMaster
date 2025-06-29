using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eProfessional.DAL.Models;
[Table("User")]
public partial class User
{
    [Key]
    [Column("IDUser")]
    public int Iduser { get; set; }

    [Column("username")]
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("passwordSalt")]
    [StringLength(255)]
    public string PasswordSalt { get; set; } = null!;

    [Column("passwordHash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("phone")]
    [StringLength(15)]
    public string? Phone { get; set; }

    [Column("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [Column("firstName")]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Column("lastName")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();

    [InverseProperty("User")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
