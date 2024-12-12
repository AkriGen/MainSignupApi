using System;
using System.Collections.Generic;

namespace WebAPINatureHub3.Models;

public partial class Bookmark
{
    public int BookmarkId { get; set; }

    public int UserId { get; set; }

    public int RemedyId { get; set; }

    public virtual Remedy Remedy { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
