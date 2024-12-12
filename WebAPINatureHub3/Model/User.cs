using System;
using System.Collections.Generic;

namespace WebAPINatureHub3.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public byte[]? UserImage { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Role Role { get; set; } = null!;
}
