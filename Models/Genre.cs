using System;
using System.Collections.Generic;

namespace IJW2.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Information { get; set; }

    public virtual ICollection<RecordsGenre> RecordsGenres { get; } = new List<RecordsGenre>();
}
