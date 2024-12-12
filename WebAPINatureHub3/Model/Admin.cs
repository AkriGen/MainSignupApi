using System;
using System.Collections.Generic;

namespace WebAPINatureHub3.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<HealthTip> HealthTips { get; set; } = new List<HealthTip>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Remedy> Remedies { get; set; } = new List<Remedy>();

    public virtual Role Role { get; set; } = null!;
}
