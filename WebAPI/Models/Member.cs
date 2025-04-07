using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public string? MemberAddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string MemberType { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual Professional? Professional { get; set; }
}
