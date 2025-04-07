using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models;

[Table("User")]
[Index("Username", Name = "UQ_User_Username", IsUnique = true)]
[Index("Email", Name = "UQ__User__AB6E6164B8D4FB09", IsUnique = true)]
[Index("Email", Name = "idxMembersEmail")]
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

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [Column("firstname")]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Column("lastname")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string Role { get; set; } = "User"!;

    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("User")]
    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
