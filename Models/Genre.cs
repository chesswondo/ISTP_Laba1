using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IJW2.Models;

public partial class Genre
{
    public int Id { get; set; }

    [Display(Name = "Жанр")]
    public string Name { get; set; } = null!;

    [Display(Name = "Інформація")]
    public string? Information { get; set; }

    public virtual ICollection<RecordsGenre> RecordsGenres { get; } = new List<RecordsGenre>();
}
