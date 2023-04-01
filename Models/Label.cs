using System;
using System.Collections.Generic;

namespace MusBase.Models;

public partial class Label
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Information { get; set; }

    public virtual ICollection<Artist> Artists { get; } = new List<Artist>();
}
