using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

[Index("CountryCode", Name = "IX_Members_CountryCode")]
[Index("RoleID", Name = "IX_Members_RoleID")]
public partial class Members
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MemberID { get; set; } = null!;

    [StringLength(40)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string CountryCode { get; set; } = null!;

    public byte RoleID { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    [ForeignKey("CountryCode")]
    [InverseProperty("Members")]
    public virtual Countries CountryCodeNavigation { get; set; } = null!;

    [InverseProperty("Member")]
    public virtual Login? Login { get; set; }

    [ForeignKey("RoleID")]
    [InverseProperty("Members")]
    public virtual MemberRoles Role { get; set; } = null!;

    [InverseProperty("Member")]
    public virtual ICollection<Visits> Visits { get; set; } = new List<Visits>();

    [InverseProperty("Member")]
    public virtual ICollection<VisitsPhotos> VisitsPhotos { get; set; } = new List<VisitsPhotos>();
}
