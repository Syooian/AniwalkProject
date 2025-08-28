using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

public partial class Animes
{
    [Key]
    [StringLength(4)]
    [Unicode(false)]
    public string AnimeID { get; set; } = null!;

    [StringLength(30)]
    public string Title { get; set; } = null!;

    [StringLength(8)]
    [Unicode(false)]
    public string HeaderPhoto { get; set; } = null!;

    [StringLength(1000)]
    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    [InverseProperty("Anime")]
    public virtual ICollection<Visits> Visits { get; set; } = new List<Visits>();
}
