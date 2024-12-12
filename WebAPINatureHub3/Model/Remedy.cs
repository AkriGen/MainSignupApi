using System;
using System.Collections.Generic;

namespace WebAPINatureHub3.Models;

public partial class Remedy
{
    public int RemedyId { get; set; }

    public string RemedyName { get; set; } = null!;

    public byte[]? Remediesimg { get; set; }

    public string? Description { get; set; }

    public string? Benefits { get; set; }

    public string? PreparationMethod { get; set; }

    public string? UsageInstructions { get; set; }

    public int CategoryId { get; set; }

    public int CreatedByAdminId { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual Category Category { get; set; } = null!;

    public virtual Admin CreatedByAdmin { get; set; } = null!;
}
