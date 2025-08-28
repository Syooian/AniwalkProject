using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

public partial class Countries
{
    [Key]
    [StringLength(3)]
    [Unicode(false)]
    public string CountryCode { get; set; } = null!;

    [StringLength(30)]
    public string CountryName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    [InverseProperty("CountryCodeNavigation")]
    public virtual ICollection<Members> Members { get; set; } = new List<Members>();

    [InverseProperty("CountryCodeNavigation")]
    public virtual ICollection<Visits> Visits { get; set; } = new List<Visits>();
}
