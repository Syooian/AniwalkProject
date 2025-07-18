using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

[Keyless]
public partial class VM_Visits
{
    [StringLength(3)]
    [Unicode(false)]
    public string CountryCode { get; set; } = null!;

    [StringLength(4)]
    [Unicode(false)]
    public string AnimeID { get; set; } = null!;

    [StringLength(4)]
    [Unicode(false)]
    public string Expr1 { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string Expr2 { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MemberID { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Expr3 { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string Expr4 { get; set; } = null!;
}
