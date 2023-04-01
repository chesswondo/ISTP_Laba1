using System;
using System.Collections.Generic;

namespace MusBase.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; } = new List<Artist>();
}
