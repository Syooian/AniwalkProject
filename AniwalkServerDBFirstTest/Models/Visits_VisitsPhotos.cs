using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

[Keyless]
public partial class Visits_VisitsPhotos
{
    public int SN { get; set; }

    public int Expr1 { get; set; }
}
