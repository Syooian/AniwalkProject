using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

[Index("MemberID", Name = "IX_VisitsPhotos_MemberID")]
[Index("SN", Name = "IX_VisitsPhotos_SN")]
public partial class VisitsPhotos
{
    [Key]
    [StringLength(36)]
    [Unicode(false)]
    public string PhotoID { get; set; } = null!;

    [StringLength(500)]
    public string Description { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MemberID { get; set; } = null!;

    public int SN { get; set; }

    [ForeignKey("MemberID")]
    [InverseProperty("VisitsPhotos")]
    public virtual Members Member { get; set; } = null!;

    [ForeignKey("SN")]
    [InverseProperty("VisitsPhotos")]
    public virtual Visits SNNavigation { get; set; } = null!;
}
