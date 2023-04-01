using System;
using System.Collections.Generic;

namespace MusBase.Models;

public partial class Artist
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? DateBase { get; set; }

    public DateTime? DateEnd { get; set; }

    public int? RelatedProjects { get; set; }

    public int? CountryId { get; set; }

    public int? LabelId { get; set; }

    public string? Information { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Label? Label { get; set; }

    public virtual ICollection<RecordsArtist> RecordsArtists { get; } = new List<RecordsArtist>();
}
