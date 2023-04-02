using System;
using System.Collections.Generic;

namespace MusBase.Models;

public partial class RecordsArtist
{
    public int Id { get; set; }

    public int ArtistId { get; set; }

    public int RecordId { get; set; }

    public virtual Artist? Artist { get; set; } = null!;

    public virtual Record? Record { get; set; } = null!;
}
