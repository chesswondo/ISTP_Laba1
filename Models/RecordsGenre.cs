using System;
using System.Collections.Generic;

namespace MusBase.Models;

public partial class RecordsGenre
{
    public int Id { get; set; }

    public int RecordId { get; set; }

    public int GenreId { get; set; }

    public virtual Genre? Genre { get; set; } = null!;

    public virtual Record? Record { get; set; } = null!;
}
