using System;
using System.Collections.Generic;

namespace WebAPINatureHub3.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public byte[]? Productimg { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int? StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public int CreatedByAdminId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category Category { get; set; } = null!;

    public virtual Admin CreatedByAdmin { get; set; } = null!;
}
