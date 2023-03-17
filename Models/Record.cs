using System;
using System.Collections.Generic;

namespace IJW2.Models;

public partial class Record
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ArtistId { get; set; }

    public DateTime? Date { get; set; }

    public string Quality { get; set; } = null!;

    public string? Information { get; set; }

    public virtual ICollection<RecordsArtist> RecordsArtists { get; } = new List<RecordsArtist>();

    public virtual ICollection<RecordsGenre> RecordsGenres { get; } = new List<RecordsGenre>();

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();
}
