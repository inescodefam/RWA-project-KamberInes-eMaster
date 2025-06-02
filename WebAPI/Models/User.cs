using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("User")]
[Index("Email", Name = "UQ__User__AB6E61645D4812AE", IsUnique = true)]
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

    [Column("memberAddress")]
    [StringLength(255)]
    public string? MemberAddress { get; set; }

    [Column("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [Column("firstName")]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Column("lastName")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("User")]
    public virtual ICollection<Professional> Professionals { get; set; } = new List<Professional>();

    [InverseProperty("User")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("User")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
