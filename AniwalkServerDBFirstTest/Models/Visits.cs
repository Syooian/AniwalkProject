using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

[Index("AnimeID", Name = "IX_Visits_AnimeID")]
[Index("CountryCode", Name = "IX_Visits_CountryCode")]
[Index("MemberID", Name = "IX_Visits_MemberID")]
public partial class Visits
{
    [Key]
    public int SN { get; set; }

    [StringLength(1000)]
    public string MainText { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime VisitedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string MemberID { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string CountryCode { get; set; } = null!;

    [StringLength(4)]
    [Unicode(false)]
    public string AnimeID { get; set; } = null!;

    [ForeignKey("AnimeID")]
    [InverseProperty("Visits")]
    public virtual Animes Anime { get; set; } = null!;

    [InverseProperty("SNNavigation")]
    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    [ForeignKey("CountryCode")]
    [InverseProperty("Visits")]
    public virtual Countries CountryCodeNavigation { get; set; } = null!;

    [ForeignKey("MemberID")]
    [InverseProperty("Visits")]
    public virtual Members Member { get; set; } = null!;

    [InverseProperty("SNNavigation")]
    public virtual ICollection<VisitsPhotos> VisitsPhotos { get; set; } = new List<VisitsPhotos>();

    [ForeignKey("VisitSN")]
    [InverseProperty("VisitSN")]
    public virtual ICollection<VisitsTags> TagSN { get; set; } = new List<VisitsTags>();
}
