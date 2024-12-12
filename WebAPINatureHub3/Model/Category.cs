using System;
using System.Collections.Generic;

namespace WebAPINatureHub3.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<HealthTip> HealthTips { get; set; } = new List<HealthTip>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Remedy> Remedies { get; set; } = new List<Remedy>();
}
