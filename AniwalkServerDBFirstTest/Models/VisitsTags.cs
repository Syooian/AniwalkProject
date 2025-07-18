using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

public partial class VisitsTags
{
    [Key]
    public int SN { get; set; }

    [StringLength(10)]
    public string Tag { get; set; } = null!;

    [ForeignKey("TagSN")]
    [InverseProperty("TagSN")]
    public virtual ICollection<Visits> VisitSN { get; set; } = new List<Visits>();
}
