using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Models;

[Index("MemberID", Name = "IX_Login_MemberID", IsUnique = true)]
public partial class Login
{
    [Key]
    [StringLength(20)]
    [Unicode(false)]
    public string Account { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string MemberID { get; set; } = null!;

    [ForeignKey("MemberID")]
    [InverseProperty("Login")]
    public virtual Members Member { get; set; } = null!;
}
