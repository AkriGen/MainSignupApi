using System;
using System.Collections.Generic;

namespace WebAPINatureHub3.Models;

public partial class HealthTip
{
    public int TipId { get; set; }

    public string TipTitle { get; set; } = null!;

    public string? TipDescription { get; set; }

    public byte[]? HealthTipsimg { get; set; }

    public int CategoryId { get; set; }

    public int CreatedByAdminId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Admin CreatedByAdmin { get; set; } = null!;
}
