using System;
using System.Collections.Generic;

namespace IJW2.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int? Rate { get; set; }

    public string? Comment { get; set; }

    public int RecordId { get; set; }

    public virtual Record Record { get; set; } = null!;
}
